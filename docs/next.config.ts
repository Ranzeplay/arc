import type { NextConfig } from 'next';
import createMDX from '@next/mdx';
import cp from 'child_process';

const nextConfig: NextConfig = {
  pageExtensions: ['js', 'jsx', 'md', 'mdx', 'ts', 'tsx'],
  env: {
    NEXT_PUBLIC_COMMIT_HASH: cp.execSync('git rev-parse --short HEAD').toString().trim(),
    NEXT_PUBLIC_BUILD_TIME: new Date().toUTCString(),
    NEXT_PUBLIC_COPYRIGHT_YEAR: new Date().getFullYear().toString(),
    NEXT_PUBLIC_NODE_ENV: process.env.NODE_ENV || 'development',
  }
};

const withMDX = createMDX({
  options: {
    remarkPlugins: [
      'remark-gfm',
      'remark-frontmatter',
      'remark-mdx-frontmatter'
    ],
    rehypePlugins: [
      [
        '@shikijs/rehype',
        {
          themes: {
            light: 'vitesse-light',
            dark: 'vitesse-dark',
          }
        }
      ],
      'rehype-slug',
      ['rehype-katex', { strict: true, throwOnError: true }],
      '@stefanprobst/rehype-extract-toc',
      '@stefanprobst/rehype-extract-toc/mdx'
    ]
  },
  extension: /\.(md|mdx)$/,
})

// Merge MDX config with Next.js config
export default withMDX(nextConfig)
