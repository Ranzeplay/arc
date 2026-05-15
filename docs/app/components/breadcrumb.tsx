import { ChevronRight } from "lucide-react";
import Link from "next/link";
import type { TreeNode } from "../[...slug]/utils";

export default function Breadcrumb({ path }: { path: TreeNode[] }) {
  return (
    <nav className="flex flex-row gap-x-1 items-center not-prose">
      {path.map((node, index) => (
        <span key={index.toString()} className="text-sm text-neutral-500 flex flex-row gap-x-1 items-center">
          {index > 0 && <ChevronRight size={16} fontWeight={400} />}
          <Link className="text-neutral-500 hover:underline dark:hover:text-neutral-400 hover:text-natural-800" href={node.path}>
            {node.title}
          </Link>
        </span>
      ))}
    </nav>
  );
}
