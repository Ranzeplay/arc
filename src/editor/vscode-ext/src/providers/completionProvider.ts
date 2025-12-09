import type * as vscode from 'vscode';
import { ArcParser } from '../parser/ArcParser';
import {
	createKeywordCompletions,
	createSnippetCompletions,
	createSymbolCompletions,
	createTypeCompletions,
	ScopeAnalyzer,
} from './completion';

/**
 * Provides code completion for Arc language
 */
export class ArcCompletionProvider implements vscode.CompletionItemProvider {
	private parser = new ArcParser();
	private scopeAnalyzer = new ScopeAnalyzer();

	provideCompletionItems(
		document: vscode.TextDocument,
		position: vscode.Position,
		_token: vscode.CancellationToken,
		_context: vscode.CompletionContext
	): vscode.ProviderResult<vscode.CompletionItem[] | vscode.CompletionList> {
		const completions: vscode.CompletionItem[] = [];

		// Check if we're completing after "self."
		const linePrefix = document
			.lineAt(position)
			.text.substr(0, position.character);
		const isSelfCompletion = /\bself\.\s*$/.test(linePrefix);

		// Add variable/field/function completions first (higher priority)
		const symbolCompletions = this.getSymbolCompletions(
			document,
			position,
			isSelfCompletion
		);
		completions.push(...symbolCompletions);

		// Add keyword completions
		completions.push(...createKeywordCompletions());

		// Add type completions
		completions.push(...createTypeCompletions());

		// Add snippet completions
		completions.push(...createSnippetCompletions());

		return completions;
	}

	/**
	 * Get symbol-based completions (variables, functions, fields)
	 */
	private getSymbolCompletions(
		document: vscode.TextDocument,
		position: vscode.Position,
		isSelfCompletion: boolean
	): vscode.CompletionItem[] {
		const text = document.getText();

		try {
			const parseResult = this.parser.parse(text);
			const visibleSymbols = this.scopeAnalyzer.getVisibleSymbols(
				parseResult.symbols,
				position,
				isSelfCompletion
			);

			return createSymbolCompletions(visibleSymbols, isSelfCompletion);
		} catch (error) {
			// If parsing fails, silently return empty array
			console.error('Error parsing for symbol completions:', error);
			return [];
		}
	}
}

