import { defineConfig } from 'vitepress'
import { withSidebar } from 'vitepress-sidebar';

// https://vitepress.dev/reference/site-config
const vitePressOptions = {
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

    socialLinks: [
      { icon: 'github', link: 'https://github.com/Ranzeplay/arc' }
    ],

    footer: {
      copyright: 'Copyright © 2023-present Jeb Feng'
    },

    search: {
      provider: 'local'
    }
  },

  lastUpdated: true
};

const vitePressSidebarOptions = {
  // VitePress Sidebar's options here...
  documentRootPath: '/',
  collapsed: false,
  capitalizeFirst: true,
  useTitleFromFrontmatter: true,
};

export default defineConfig(withSidebar(vitePressOptions, vitePressSidebarOptions));
