import { defineConfig } from 'vitepress'

// https://vitepress.dev/reference/site-config
export default defineConfig({
  title: "TAPL Docs",
  description: "The documentation site of The Arc Programming Language",
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    outline: [2, 3],
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Development', link: '/development' },
      { text: 'Usage', link: '/usage' },
      { text: 'References', link: '/reference' },
      { text: 'Stdlib', link: '/stdlib' }
    ],

    sidebar: {
      '/development/': [
        {
          text: 'Development',
          items: [
            { text: 'Index', link: '/development/' },
            { text: 'Relocation generation', link: '/development/relocation-generation' },
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
      '/stdlib/': [
        {
          text: 'Stdlib',
          items: [
            { text: 'Index', link: '/stdlib/' },
            { text: 'Console', link: '/stdlib/console' },
          ]
        }
      ],
      '/reference/': [
        {
          text: 'References',
          items: [
            { text: 'Index', link: '/reference/' },
            { text: 'Instruction set', link: '/reference/instruction-set' },
            { text: 'Package descriptor', link: '/reference/package-descriptor' },
            { text: 'Symbols', link: '/reference/symbol' },
            { text: 'Data type', link: '/reference/data-type' },
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
