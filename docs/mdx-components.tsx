import type { MDXComponents } from 'mdx/types'
import GrammarSource from './app/components/grammarSource'
 
const components: MDXComponents = { GrammarSource }
 
export function useMDXComponents(): MDXComponents {
  return components
}
