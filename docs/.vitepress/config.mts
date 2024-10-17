import { defineConfig } from 'vitepress'

// https://vitepress.dev/reference/site-config
export default defineConfig({
  title: "TAPL Docs",
  description: "The documentation site of The Arc Programming Language",
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Development', link: '/development' },
      { text: 'Usage', link: '/usage' },
      { text: 'References', link: '/reference' }
    ],

    sidebar: {
      '/development/': [
        {
          text: 'Development',
          items: [
            { text: 'Index', link: '/development/' },
          ]
        }
      ],
      '/usage/': [
        {
          text: 'Usage',
          items: [
            { text: 'Index', link: '/usage/' },
          ]
        }
      ],
      '/reference/': [
        {
          text: 'References',
          items: [
            { text: 'Index', link: '/reference/' },
            { text: 'Instruction set', link: '/reference/instruction-set' },
            { text: 'Descriptor set', link: '/reference/descriptor-set' },
            { text: 'On-stack data save-load operations', link: '/reference/on-stack-data-sl-operations' },
            { text: 'Signature', link: '/reference/signature' },
          ]
        }
      ]
    },

    socialLinks: [
      { icon: 'github', link: 'https://github.com/Ranzeplay/arc' }
    ]
  }
})
