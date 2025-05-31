import type { BaseLayoutProps, LinkItemType } from 'fumadocs-ui/layouts/shared';
import { FolderCog, GitBranch, GraduationCap, Library, Rocket } from 'lucide-react';

export const linkItems: LinkItemType[] = [
  {
    text: 'Usage',
    url: '/docs/usage',
    icon: <Rocket />
  },
  {
    text: 'Stdlib',
    url: '/docs/stdlib',
    icon: <Library />
  },
  {
    text: 'Reference',
    url: '/docs/reference',
    icon: <FolderCog />
  },
  {
    text: 'Development',
    url: '/docs/development',
    icon: <GitBranch />
  }
]

/**
 * Shared layout configurations
 *
 * you can customise layouts individually from:
 * Home Layout: app/(home)/layout.tsx
 * Docs Layout: app/docs/layout.tsx
 */
export const baseOptions: BaseLayoutProps = {
  nav: {
    title: (
      <span className='flex items-center gap-2'>
        <GraduationCap />
        Arc Docs
      </span>
    ),
    transparentMode: 'top',
  },
  links: linkItems,
};
