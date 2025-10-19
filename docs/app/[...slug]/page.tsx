import path from "path";
import { generateDirectoryTree, TreeNode } from "./utils";
import Directory from "@/app/components/directory";
import NotFound from "@/app/not-found";
import Breadcrumb from "../components/breadcrumb";
import React from "react";
import TableOfContents from "../components/toc";

// Cache directory tree at build time
const directoryTree = generateDirectoryTree();

export async function generateStaticParams() {
  const params: { slug: string[] }[] = [];
  const CONTENT_DIR = path.join(process.cwd(), "app", "content");
  const fs = await import("fs");

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

export default async function Page({
  params,
}: {
  params: Promise<{ slug: string[] }>;
}) {
  const { slug } = await params;
  const slugPath = slug.join("/");

  // Direct static import - Next.js will handle this at build time
  let Content, toc;
  try {
    const module = await import(`@/app/content/${slugPath}.mdx`);
    Content = module.default;
    toc = module.tableOfContents;
  } catch {
    try {
      const module = await import(`@/app/content/${slugPath}/index.mdx`);
      Content = module.default;
      toc = module.tableOfContents;
    } catch {
      return <NotFound />;
    }
  }

  let rootNode = directoryTree.find((dir) => dir.path === "/" + slug[0])!;
  let path = findCurrentNodePath(rootNode, slug)!;
  let currentNode = path.at(-1) || rootNode;
  path = [rootNode, ...path];

  return (
    <main className="flex flex-row divide-x-1 divide-neutral-200 flex-1 h-full">
      <div className="basis-3/13 sticky top-14 h-[calc(100vh-3.5rem)] overflow-y-auto p-8 shadow flex flex-col gap-y-5">
        <TableOfContents toc={toc} />
        <div className="w-full h-px bg-neutral-300" />
        <Directory node={rootNode} currentPath={"/" + slugPath} />
      </div>
      <div className="prose p-8 overflow-y-auto grow w-full max-w-full">
        {path.length > 1 && <Breadcrumb path={path} />}
        <h1 className="font-serif mt-3 mb-1!">{currentNode.title}</h1>
        <p className="mt-0! text-neutral-500">Last updated at: {currentNode.lastModificationTime?.toLocaleString()}</p>
        <div className="w-full h-px bg-neutral-300" />
        <Content />
      </div>
    </main>
  );
}

function findCurrentNodePath(
  root: TreeNode,
  slugParts: string[]
): TreeNode[] | undefined {
  const path: TreeNode[] = [];
  let currentNode: TreeNode | undefined = root;

  for (let i = 1; i < slugParts.length; i++) {
    const concatenatedPath = "/" + slugParts.slice(0, i + 1).join("/");
    if (!currentNode) break;
    const nextNode: TreeNode | undefined = currentNode.children?.find(
      (child) => child.path === concatenatedPath
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
