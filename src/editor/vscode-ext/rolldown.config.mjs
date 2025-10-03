import { defineConfig } from 'rolldown';

export default defineConfig({
	input: './src/extension.ts',
	output: {
		dir: 'dist',
		format: 'cjs',
		entryFileNames: 'extension.js',
	},
	external: ['vscode'],
	platform: 'node',
	resolve: {
		extensions: ['.ts', '.js'],
	},
	plugins: [
		// TypeScript compilation is handled automatically by rolldown
	],
	define: {
		// Ensure NODE_ENV is defined for production builds
		'process.env.NODE_ENV': JSON.stringify(
			process.env.NODE_ENV || 'development'
		),
	},
});
