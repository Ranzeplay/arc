/** biome-ignore-all lint/suspicious/noTemplateCurlyInString: it just works */

import * as vscode from 'vscode';
import { type Symbol as ArcSymbol, SymbolKind } from '../../parser/ArcParser';
import { ARC_KEYWORDS, ARC_TYPES } from './constants';

/**
 * Create keyword completion items
 */
export function createKeywordCompletions(): vscode.CompletionItem[] {
	return ARC_KEYWORDS.map(keyword => {
		const item = new vscode.CompletionItem(
			keyword,
			vscode.CompletionItemKind.Keyword
		);
		item.detail = 'Arc keyword';
		item.sortText = `z_${keyword}`; // Lower priority
		return item;
	});
}

/**
 * Create type completion items
 */
export function createTypeCompletions(): vscode.CompletionItem[] {
	return ARC_TYPES.map(type => {
		const item = new vscode.CompletionItem(
			type,
			vscode.CompletionItemKind.TypeParameter
		);
		item.detail = 'Arc base type';
		item.sortText = `z_${type}`; // Lower priority
		return item;
	});
}

/**
 * Create snippet completion items
 */
export function createSnippetCompletions(): vscode.CompletionItem[] {
	const snippets: vscode.CompletionItem[] = [];

	const functionSnippet = new vscode.CompletionItem(
		'func',
		vscode.CompletionItemKind.Snippet
	);
	functionSnippet.insertText = new vscode.SnippetString(
		'func ${1:name}(${2:params}): ${3:returnType} {\n\t${4:// body}\n}'
	);
	functionSnippet.detail = 'Function declaration';
	functionSnippet.sortText = 'zz_func'; // Lowest priority
	snippets.push(functionSnippet);

	const groupSnippet = new vscode.CompletionItem(
		'group',
		vscode.CompletionItemKind.Snippet
	);
	groupSnippet.insertText = new vscode.SnippetString(
		'group ${1:Name} {\n\t${2:// members}\n}'
	);
	groupSnippet.detail = 'Group declaration';
	groupSnippet.sortText = 'zz_group'; // Lowest priority
	snippets.push(groupSnippet);

	return snippets;
}

/**
 * Create completion items from symbols
 */
export function createSymbolCompletions(
	symbols: ArcSymbol[],
	isSelfCompletion: boolean
): vscode.CompletionItem[] {
	return symbols.map(symbol => {
		const itemKind = getCompletionItemKind(symbol.kind, isSelfCompletion);
		const item = new vscode.CompletionItem(symbol.name, itemKind);
		item.detail = getSymbolKindLabel(symbol.kind);
		return item;
	});
}

/**
 * Get VS Code completion item kind from Arc symbol kind
 */
function getCompletionItemKind(
	kind: SymbolKind,
	isSelfCompletion: boolean
): vscode.CompletionItemKind {
	if (isSelfCompletion && kind === SymbolKind.Field) {
		return vscode.CompletionItemKind.Field;
	}
	switch (kind) {
		case SymbolKind.Function:
		case SymbolKind.SelfFunction:
			return vscode.CompletionItemKind.Function;
		case SymbolKind.Field:
			return vscode.CompletionItemKind.Field;
		default:
			return vscode.CompletionItemKind.Variable;
	}
}

/**
 * Get human-readable label for symbol kind
 */
function getSymbolKindLabel(kind: SymbolKind): string {
	switch (kind) {
		case SymbolKind.Variable:
			return 'Variable';
		case SymbolKind.Field:
			return 'Field';
		case SymbolKind.Function:
		case SymbolKind.SelfFunction:
			return 'Function';
		default:
			return 'Symbol';
	}
}

