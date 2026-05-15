import Link from "next/link";
import type { TreeNode } from "../[...slug]/utils";

function DirectoryItem({
  node,
  currentPath,
}: { node: TreeNode; currentPath: string }) {
  return (
    <li className="my-0.5 py-1.5 px-2 rounded-lg hover:bg-neutral-100 dark:hover:bg-neutral-900 transition-all">
      <Link
        href={`${node.path}`}
        className={`${currentPath === node.path ? "font-medium text-neutral-800 dark:text-neutral-100" : "text-neutral-600 dark:text-neutral-300"}`}
      >
        {node.title}
      </Link>
      {node.children && node.children.length > 0 && (
        <ul className="ml-4 mt-1 border-l border-neutral-200 dark:border-neutral-700 pl-3">
          {node.children.map((child) => (
            <DirectoryItem key={child.path} node={child} currentPath={currentPath} />
          ))}
        </ul>
      )}
    </li>
  );
}

export default function Directory({
  node,
  currentPath,
}: { node: TreeNode; currentPath: string }) {
  return (
    <nav>
      <Link
        className="font-semibold text-lg hover:underline dark:text-neutral-100"
        href={node.path}
      >
        {node.title}
      </Link>
      <ul className="text-sm mt-4">
        {node.children?.map((child) => (
          <DirectoryItem key={child.path} node={child} currentPath={currentPath} />
        ))}
      </ul>
    </nav>
  );
}
