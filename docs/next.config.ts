import type { NextConfig } from 'next';
import createMDX from '@next/mdx';

const nextConfig: NextConfig = {
  pageExtensions: ['js', 'jsx', 'md', 'mdx', 'ts', 'tsx'],
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
      ['rehype-katex', { strict: true, throwOnError: true }]
    ]
  },
  extension: /\.(md|mdx)$/,
})

// Merge MDX config with Next.js config
export default withMDX(nextConfig)
