"use client";

import { ChevronDown } from "lucide-react";
import Link from "next/link";
import { useEffect, useState } from "react";
import type { TreeNode } from "../[...slug]/utils";

function DirectoryItem({
  node,
  currentPath,
}: { node: TreeNode; currentPath: string }) {
  const isActive = currentPath === node.path;
  const children = node.children ?? [];
  const hasChildren = children.length > 0;
  const isInActiveBranch = currentPath.startsWith(`${node.path}/`);
  const [expand, setExpand] = useState(isInActiveBranch);

  useEffect(() => {
    if (isInActiveBranch) {
      setExpand(true);
    }
  }, [isInActiveBranch]);

  const [isHovering, setIsHovering] = useState(false);

  return (
    <li className="my-0.5">
      {/** biome-ignore lint/a11y/noStaticElementInteractions: This div is used to group the link and the expand button, and it handles the hover state for both elements. */}
      <div className="flex flex-row items-center gap-x-1 hover:bg-neutral-100 dark:hover:bg-neutral-900 rounded-lg transition-all" onMouseEnter={() => setIsHovering(true)} onMouseLeave={() => setIsHovering(false)}>
        <Link
          href={`${node.path}`}
          aria-current={isActive ? "page" : undefined}
          className={`block py-1.5 px-2 transition-all grow ${
            isActive
              ? "font-medium bg-neutral-100 text-neutral-800 dark:bg-neutral-900 dark:text-neutral-100"
              : "text-neutral-600 dark:text-neutral-300"
          } ${isInActiveBranch && !isActive && !expand ? "underline" : ""}`}
        >
          {node.title}
        </Link>
        {hasChildren && isHovering && (
          <button
            type="button"
            aria-label={expand ? `Collapse ${node.title}` : `Expand ${node.title}`}
            className="rounded-md p-1 text-neutral-500 dark:text-neutral-400"
            onClick={() => setExpand(!expand)}
          >
            <ChevronDown className={`size-4 transition-transform mr-1 cursor-pointer ${!expand ? "rotate-90" : ""}`} />
          </button>
        )}
      </div>
      {hasChildren && expand && (
        <ul className="ml-4 mt-1 border-l border-neutral-200 dark:border-neutral-700 pl-3">
          {children.map((child) => (
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
  const isInActiveBranch = currentPath.startsWith(`${node.path}/`);
  const [expand, setExpand] = useState(true);

  return (
    <nav>
      <div className="flex flex-row justify-between items-center">
        <Link
          className="font-semibold text-lg hover:underline dark:text-neutral-100"
          href={node.path}
        >
          {node.title}
        </Link>
        <button
          type="button"
          aria-label={expand ? "Collapse directory" : "Expand directory"}
          className="text-neutral-500 dark:text-neutral-400 text-sm hover:underline cursor-pointer"
          onClick={() => setExpand(!expand)}
        >
          <ChevronDown className={`transition-transform ${!expand ? "rotate-180" : ""}`} />
        </button>
      </div>
      <div className={expand ? "" : "hidden"}>
        <ul className="text-sm mt-4">
          {node.children?.map((child) => (
            <DirectoryItem key={child.path} node={child} currentPath={currentPath} />
          ))}
        </ul>
      </div>
    </nav>
  );
}
