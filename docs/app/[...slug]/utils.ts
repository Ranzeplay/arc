import fs from 'fs';
import path from 'path';
import matter from 'gray-matter';
import cp from 'child_process';

export interface TreeNode {
  title: string;
  path: string;
  children?: TreeNode[];
  lastModificationTime?: Date;
}

export function generateDirectoryTree(): TreeNode[] {
  const contentDir = path.join(process.cwd(), 'app', 'content');
  
  function buildTree(dir: string, basePath: string[] = []): TreeNode[] {
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    const nodes: TreeNode[] = [];

    // Sort entries: directories first, then files
    entries.sort((a, b) => {
      if (a.isDirectory() && !b.isDirectory()) return -1;
      if (!a.isDirectory() && b.isDirectory()) return 1;
      return a.name.localeCompare(b.name);
    });

    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);

      if (entry.isDirectory()) {
        // Check if directory has an index.mdx
        const indexPath = path.join(fullPath, 'index.mdx');
        const hasIndex = fs.existsSync(indexPath);

        let title = entry.name;
        let nodePath = "/" + [...basePath, entry.name].join('/');

        if (hasIndex) {
          // Read frontmatter from index.mdx
          const content = fs.readFileSync(indexPath, 'utf-8');
          const { data } = matter(content);
          title = data.title || entry.name;
        }

        const children = buildTree(fullPath, [...basePath, entry.name]);
        
        nodes.push({
          title,
          path: nodePath,
          children: children.length > 0 ? children : undefined
        });
      } else if (entry.name.endsWith('.mdx') && entry.name !== 'index.mdx') {
        // Read frontmatter from the MDX file
        const content = fs.readFileSync(fullPath, 'utf-8');
        const { data } = matter(content);
        const fileName = entry.name.replace('.mdx', '');
        const lastModificationTimeOnGit = cp.execSync(`git log -1 --format="%cI" -- "${fullPath}"`).toString().trim();
        
        nodes.push({
          title: data.title || fileName,
          path: "/" + [...basePath, fileName].join('/'),
          lastModificationTime: lastModificationTimeOnGit ? new Date(lastModificationTimeOnGit) : undefined
        });
      }
    }

    return nodes;
  }

  return buildTree(contentDir);
}
