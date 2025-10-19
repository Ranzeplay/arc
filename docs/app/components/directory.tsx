import Link from 'next/link';
import { TreeNode } from '../[...slug]/utils';

function DirectoryItem({ node }: { node: TreeNode }) {
  return (
    <li className="my-1">
      <Link 
        href={`/${node.path}`}
        className="hover:text-blue-600 hover:underline"
      >
        {node.title}
      </Link>
      {node.children && node.children.length > 0 && (
        <ul className="ml-4 mt-1 border-l border-neutral-200 pl-3">
          {node.children.map((child, index) => (
            <DirectoryItem key={index} node={child} />
          ))}
        </ul>
      )}
    </li>
  );
}

export default function Directory({ tree }: { tree: TreeNode[] }) {
  return (
    <nav>
      <h2 className="font-bold text-lg mb-4">Directory</h2>
      <ul className="text-sm">
        {tree.map((node, index) => (
          <DirectoryItem key={index} node={node} />
        ))}
      </ul>
    </nav>
  );
}
