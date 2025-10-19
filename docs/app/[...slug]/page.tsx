import fs from 'fs';
import path from 'path';
import { generateDirectoryTree } from './utils';
import Directory from '@/app/components/directory';

export async function generateStaticParams() {
  const contentDir = path.join(process.cwd(), 'app', 'content');
  const params: { slug: string[] }[] = [];

  function getAllMdxFiles(dir: string, basePath: string[] = []): void {
    const entries = fs.readdirSync(dir, { withFileTypes: true });

    for (const entry of entries) {
      if (entry.isDirectory()) {
        getAllMdxFiles(path.join(dir, entry.name), [...basePath, entry.name]);
      } else if (entry.name.endsWith('.mdx')) {
        const fileName = entry.name.replace('.mdx', '');
        if (fileName === 'index') {
          // For index files, use the directory path
          if (basePath.length > 0) {
            params.push({ slug: basePath });
          }
        } else {
          params.push({ slug: [...basePath, fileName] });
        }
      }
    }
  }

  getAllMdxFiles(contentDir);
  return params;
}

export default async function Page({ params }: { params: Promise<{ slug: string[] }> }) {
  const { slug } = await params;
  
  const contentDir = path.join(process.cwd(), 'app', 'content');
  const filePath = path.join(contentDir, ...slug);
  
  let Content: any;
  
  // Check if direct .mdx file exists
  const directFile = `${filePath}.mdx`;
  const indexFile = path.join(filePath, 'index.mdx');
  
  if (fs.existsSync(directFile)) {
    const module = await import(`@/app/content/${slug.join("/")}.mdx`);
    Content = module.default;
  } else if (fs.existsSync(indexFile)) {
    const module = await import(`@/app/content/${slug.join("/")}/index.mdx`);
    Content = module.default;
  } else {
    throw new Error(`Content not found for slug: ${slug.join("/")}`);
  }

  // Generate directory tree
  const directoryTree = generateDirectoryTree();

  return (
    <main className="flex flex-row divide-x-1 divide-neutral-200 flex-1 h-full">
      <div className="basis-1/5 sticky top-14 h-[calc(100vh-3.5rem)] overflow-y-auto p-8">
        <Directory tree={directoryTree} />
      </div>
      <div className="prose p-8 overflow-y-auto"><Content /></div>
    </main>
  );
}
