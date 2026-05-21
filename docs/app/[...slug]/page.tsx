/** biome-ignore-all lint/suspicious/noImplicitAnyLet: The code is organized */
import path from "node:path";
import type { Metadata } from "next";
import NotFound from "@/app/not-found";
import Breadcrumb from "../components/breadcrumb";
import ResizableSidebar from "../components/resizableSidebar";
import { generateDirectoryTree, type TreeNode } from "./utils";

import "katex/dist/katex.min.css";

// Cache directory tree at build time
const directoryTree = generateDirectoryTree();

export async function generateStaticParams() {
  const params: { slug: string[] }[] = [];
  const CONTENT_DIR = path.join(process.cwd(), "app", "content");
  const fs = await import("node:fs");

  function getAllMdxFiles(dir: string, basePath: string[] = []): void {
    const entries = fs.readdirSync(dir, { withFileTypes: true });

    for (const entry of entries) {
      if (entry.isDirectory()) {
        getAllMdxFiles(path.join(dir, entry.name), [...basePath, entry.name]);
      } else if (entry.name.endsWith(".mdx")) {
        const fileName = entry.name.replace(".mdx", "");
        params.push({
          slug:
            fileName === "index" && basePath.length > 0
              ? basePath
              : [...basePath, fileName],
        });
      }
    }
  }

  getAllMdxFiles(CONTENT_DIR);
  return params;
}

export async function generateMetadata({
  params,
}: {
  params: Promise<{ slug: string[] }>;
}): Promise<Metadata> {
  const { slug } = await params;

  if (slug.length === 0) {
    return { title: "The Arc Programming Language" };
  }

  const rootNode = directoryTree.find((dir) => dir.path === `/${slug[0]}`);

  if (!rootNode) {
    return { title: "Not Found" };
  }

  const nodePath = findCurrentNodePath(rootNode, slug);
  const currentNode = nodePath?.at(-1) || rootNode;

  return {
    title: currentNode.title,
  };
}

export default async function Page({
  params,
}: {
  params: Promise<{ slug: string[] }>;
}) {
  const { slug } = await params;
  const slugPath = slug.join("/");

  // Direct static import - Next.js will handle this at build time
  // biome-ignore lint/suspicious/noExplicitAny: dynamic import result
  let Content: any,
    toc: any = [];
  try {
    const module = await import(`@/app/content/${slugPath}.mdx`);
    Content = module.default;
    toc = module.tableOfContents || [];
  } catch {
    try {
      const module = await import(`@/app/content/${slugPath}/index.mdx`);
      Content = module.default;
      toc = module.tableOfContents || [];
    } catch {
      return <NotFound />;
    }
  }

  // biome-ignore lint/style/noNonNullAssertion: checked above
  const rootNode = directoryTree.find((dir) => dir.path === `/${slug[0]}`)!;
  // biome-ignore lint/style/noNonNullAssertion: checked above
  let path = findCurrentNodePath(rootNode, slug)!;
  const currentNode = path.at(-1) || rootNode;
  path = [rootNode, ...path];
  const breadcrumbPath = path.slice(0, -1);

  return (
    <main className="flex flex-row divide-x divide-neutral-200 dark:divide-neutral-800 flex-1 h-full">
      <ResizableSidebar
        toc={toc}
        rootNode={rootNode}
        currentPath={`/${slugPath}`}
      />
      <div className="prose dark:prose-invert p-8 overflow-y-auto grow w-full max-w-full">
        {breadcrumbPath.length > 1 && <Breadcrumb path={breadcrumbPath} />}
        <h1 className="font-serif mt-3 mb-1!">{currentNode.title}</h1>
        <p className={`mt-0! text-neutral-500 dark:text-neutral-400 ${currentNode.lastModificationTime ? 'block' : 'hidden'}`}>
          Last updated at: {currentNode.lastModificationTime?.toLocaleString()}
        </p>
        <div className="w-full h-px bg-neutral-300 dark:bg-neutral-700 my-4" />
        <Content />
      </div>
    </main>
  );
}

function findCurrentNodePath(
  root: TreeNode,
  slugParts: string[],
): TreeNode[] | undefined {
  const path: TreeNode[] = [];
  let currentNode: TreeNode | undefined = root;

  for (let i = 1; i < slugParts.length; i++) {
    const concatenatedPath = `/${slugParts.slice(0, i + 1).join("/")}`;
    if (!currentNode) break;
    const nextNode: TreeNode | undefined = currentNode.children?.find(
      (child) => child.path === concatenatedPath,
    );
    if (nextNode) {
      path.push(nextNode);
      currentNode = nextNode;
    } else {
      break;
    }
  }

  return path.length === slugParts.length - 1 ? path : undefined;
}
