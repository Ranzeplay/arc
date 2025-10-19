# The Arc Programming Language Documentation

Welcome to the official documentation site for the Arc programming language! This repository contains the source code for the Arc documentation website, built with [Next.js](https://nextjs.org/).

## 📖 About

This documentation site provides comprehensive guides, references, and examples for the Arc programming language, including:

- **Getting Started**: Installation, setup, and first steps
- **Language Reference**: Detailed documentation of Arc's syntax and features
- **Standard Library**: Documentation for Arc's built-in libraries
- **Development Guide**: Contributing to Arc and building from source

## 🚀 Quick Start

### Prerequisites

- [Node.js](https://nodejs.org/) (version 18 or higher)
- [pnpm](https://pnpm.io/) (recommended package manager)

### Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/Ranzeplay/arc
   cd arc/docs
   ```

2. Install dependencies:
   ```bash
   pnpm install
   ```

3. Start the development server:
   ```bash
   pnpm dev
   ```

4. Open [http://localhost:3000](http://localhost:3000) in your browser

## 🛠️ Available Scripts

- `pnpm dev` - Start the development server with Turbopack
- `pnpm build` - Build the application for production
- `pnpm start` - Start the production server
- `pnpm postinstall` - Process MDX files (runs automatically after install)

## 📁 Project Structure

```
arc-docs/
├── app/                   # Next.js app directory
│   ├── (home)/            # Home page route group
│   ├── docs/              # Documentation pages
│   ├── api/               # API routes (search)
│   ├── global.css         # Global styles
│   ├── layout.tsx         # Root layout
│   └── layout.config.tsx  # Layout configuration
├── content/               # MDX documentation content
│   └── docs/              # Documentation files
│       ├── development/   # Development guides
│       ├── reference/     # Language reference
│       ├── stdlib/        # Standard library docs
│       └── usage/         # Usage examples
├── lib/                   # Utility libraries
├── source.config.ts       # Fumadocs configuration
├── next.config.mjs        # Next.js configuration
└── package.json           # Project dependencies
```

## 📝 Writing Documentation

Documentation is written in MDX format and stored in the `content/docs/` directory. Each MDX file should include frontmatter with at least a title:

```mdx
---
title: Your Page Title
description: Optional description
---

# Your content here

This supports both Markdown and React components.
```

### Adding New Pages

1. Create a new `.mdx` file in the appropriate directory under `content/docs/`
2. Add the page to the relevant `meta.json` file for navigation
3. The page will automatically be included in the site navigation

### Mathematical Expressions

The site supports LaTeX mathematical expressions using KaTeX:

```mdx
Inline math: $E = mc^2$

Block math:
$$
\int_{-\infty}^{\infty} e^{-x^2} dx = \sqrt{\pi}
$$
```

## 🎨 Features

- **Fast**: Built with Next.js 15 and Turbopack for optimal performance
- **Modern UI**: Clean, responsive design powered by Fumadocs UI
- **Math Support**: LaTeX expressions rendered with KaTeX
- **Search**: Full-text search functionality
- **Dark Mode**: Automatic dark/light theme switching
- **Mobile Friendly**: Responsive design that works on all devices

## 🧰 Tech Stack

- **Framework**: [Next.js 15](https://nextjs.org/)
- **Documentation**: [Fumadocs](https://fumadocs.vercel.app/)
- **Styling**: [Tailwind CSS](https://tailwindcss.com/)
- **Content**: [MDX](https://mdxjs.com/)
- **Math**: [KaTeX](https://katex.org/)
- **Icons**: [Lucide React](https://lucide.dev/)
- **Package Manager**: [pnpm](https://pnpm.io/)

## 🔗 Related Links

- [Arc Programming Language Repository](https://github.com/Ranzeplay/arc)
- [Fumadocs Documentation](https://fumadocs.vercel.app/)

## 🤝 Contributing

We welcome contributions to improve the documentation! Here's how you can help:

1. **Fork** this repository
2. **Create** a new branch for your changes
3. **Make** your improvements (fix typos, add examples, improve explanations)
4. **Test** your changes locally with `pnpm dev`
5. **Submit** a pull request

### Content Guidelines

- Use clear, concise language
- Include practical examples
- Follow the existing documentation structure
- Test code examples to ensure they work
- Add appropriate frontmatter to new pages

## 📄 License

This documentation is part of the Arc programming language project. Please refer to the main repository for licensing information.

## 🆘 Support

If you have questions about Arc or find issues with the documentation:

- **Issues**: Open an issue in the [main Arc repository](https://github.com/Ranzeplay/arc/issues)
- **Discussions**: Join discussions in the Arc community
- **Documentation Issues**: Open an issue specifically for documentation problems

---

Built with ❤️ by the Arc programming language team.
