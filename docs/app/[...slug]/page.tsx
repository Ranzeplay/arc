import path from 'path';
import { generateDirectoryTree } from './utils';
import Directory from '@/app/components/directory';

// Cache directory tree at build time
const directoryTree = generateDirectoryTree();

export async function generateStaticParams() {
  const params: { slug: string[] }[] = [];
  const CONTENT_DIR = path.join(process.cwd(), 'app', 'content');
  const fs = await import('fs');

  function getAllMdxFiles(dir: string, basePath: string[] = []): void {
    const entries = fs.readdirSync(dir, { withFileTypes: true });

    for (const entry of entries) {
      if (entry.isDirectory()) {
        getAllMdxFiles(path.join(dir, entry.name), [...basePath, entry.name]);
      } else if (entry.name.endsWith('.mdx')) {
        const fileName = entry.name.replace('.mdx', '');
        params.push({
          slug: fileName === 'index' && basePath.length > 0 
            ? basePath 
            : [...basePath, fileName]
        });
      }
    }
  }

  getAllMdxFiles(CONTENT_DIR);
  return params;
}

export default async function Page({ 
  params 
}: { 
  params: Promise<{ slug: string[] }> 
}) {
  const { slug } = await params;
  const slugPath = slug.join('/');
  
  // Direct static import - Next.js will handle this at build time
  let Content;
  try {
    const module = await import(`@/app/content/${slugPath}.mdx`);
    Content = module.default;
  } catch {
    const module = await import(`@/app/content/${slugPath}/index.mdx`);
    Content = module.default;
  }

  return (
    <main className="flex flex-row divide-x-1 divide-neutral-200 flex-1 h-full">
      <div className="basis-1/5 sticky top-14 h-[calc(100vh-3.5rem)] overflow-y-auto p-8">
        <Directory tree={directoryTree} />
      </div>
      <div className="prose p-8 overflow-y-auto">
        <Content />
      </div>
    </main>
  );
}
