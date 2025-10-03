import * as vscode from 'vscode';

export class ArcCompletionProvider implements vscode.CompletionItemProvider {
	private keywords = [
		'if',
		'elif',
		'else',
		'while',
		'for',
		'loop',
		'foreach',
		'in',
		'break',
		'continue',
		'return',
		'throw',
		'try',
		'catch',
		'finally',
		'match',
		'case',
		'get',
		'set',
		'default',
		'await',
		'async',
		'func',
		'group',
		'field',
		'constructor',
		'destructor',
		'operator',
		'public',
		'internal',
		'protected',
		'private',
		'static',
		'const',
		'var',
		'val',
		'ref',
		'clone',
		'call',
		'new',
		'defer',
		'macro',
		'namespace',
		'link',
		'true',
		'false',
		'none',
		'any',
		'infer',
		'char',
		'bool',
		'byte',
		'int',
		'decimal',
		'string',
		'dispose',
		'with',
		'lifetime',
		'typeof',
		'self',
		'enum',
	];

	private types = [
		'int',
		'decimal',
		'char',
		'string',
		'bool',
		'byte',
		'none',
		'any',
		'infer',
	];

	provideCompletionItems(
		document: vscode.TextDocument,
		position: vscode.Position,
		token: vscode.CancellationToken,
		context: vscode.CompletionContext
	): vscode.ProviderResult<vscode.CompletionItem[] | vscode.CompletionList> {
		const completions: vscode.CompletionItem[] = [];

		// Add keyword completions
		this.keywords.forEach(keyword => {
			const item = new vscode.CompletionItem(
				keyword,
				vscode.CompletionItemKind.Keyword
			);
			item.detail = `Arc keyword`;
			completions.push(item);
		});

		// Add type completions
		this.types.forEach(type => {
			const item = new vscode.CompletionItem(
				type,
				vscode.CompletionItemKind.TypeParameter
			);
			item.detail = `Arc base type`;
			completions.push(item);
		});

		// Add snippet completions
		const functionSnippet = new vscode.CompletionItem(
			'func',
			vscode.CompletionItemKind.Snippet
		);
		functionSnippet.insertText = new vscode.SnippetString(
			'func ${1:name}(${2:params}): ${3:returnType} {\n\t${4:// body}\n}'
		);
		functionSnippet.detail = 'Function declaration';
		completions.push(functionSnippet);

		const groupSnippet = new vscode.CompletionItem(
			'group',
			vscode.CompletionItemKind.Snippet
		);
		groupSnippet.insertText = new vscode.SnippetString(
			'group ${1:Name} {\n\t${2:// members}\n}'
		);
		groupSnippet.detail = 'Group declaration';
		completions.push(groupSnippet);

		// If in a function, suggest variables
		const linePrefix = document
			.lineAt(position)
			.text.substr(0, position.character);

		return completions;
	}
}
