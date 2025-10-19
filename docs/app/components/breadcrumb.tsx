import { ChevronRight } from "lucide-react";
import Link from "next/link";
import { TreeNode } from "../[...slug]/utils";

export default function Breadcrumb({ path }: { path: TreeNode[] }) {
  return (
    <nav className="flex flex-row gap-x-1 items-center not-prose">
      {path.map((node, index) => (
        <span key={index} className="text-sm text-neutral-500 flex flex-row gap-x-1 items-center">
          {index > 0 && <ChevronRight size={16} fontWeight={400} />}
          <Link className="text-neutral-500 hover:underline hover:text-neutral-800" href={node.path}>
            {node.title}
          </Link>
        </span>
      ))}
    </nav>
  );
}
