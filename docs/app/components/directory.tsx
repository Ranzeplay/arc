import Link from 'next/link';
import { TreeNode } from '../[...slug]/utils';

function DirectoryItem({ node, currentPath }: { node: TreeNode, currentPath: string }) {
  return (
    <li className="my-1 p-2 rounded-lg hover:bg-neutral-100 transition-all">
      <Link 
        href={`${node.path}`}
        className={`${currentPath === node.path ? 'font-semibold text-neutral-800' : 'text-neutral-600'}`}
      >
        {node.title}
      </Link>
      {node.children && node.children.length > 0 && (
        <ul className="ml-4 mt-1 border-l border-neutral-200 pl-3">
          {node.children.map((child, index) => (
            <DirectoryItem key={index} node={child} currentPath={currentPath} />
          ))}
        </ul>
      )}
    </li>
  );
}

export default function Directory({ node, currentPath }: { node: TreeNode, currentPath: string }) {
  return (
    <nav>
      <Link className="font-bold text-lg hover:underline" href={node.path}>{node.title}</Link>
      <ul className="text-sm mt-4">
        {node.children?.map((child, index) => (
          <DirectoryItem key={index} node={child} currentPath={currentPath} />
        ))}
      </ul>
    </nav>
  );
}
