import type * as vscode from 'vscode';
import { type Symbol as ArcSymbol, SymbolKind } from '../../parser/ArcParser';

/**
 * Analyzes symbol visibility based on scope rules
 */
export class ScopeAnalyzer {
	/**
	 * Get all symbols visible at a given position in the document
	 */
	getVisibleSymbols(
		symbols: ArcSymbol[],
		position: vscode.Position,
		isSelfCompletion: boolean
	): ArcSymbol[] {
		const visibleSymbols: ArcSymbol[] = [];
		this.collectVisibleSymbols(
			symbols,
			position,
			isSelfCompletion,
			visibleSymbols,
			false
		);
		return visibleSymbols;
	}

	/**
	 * Recursively collect visible symbols based on scope and position
	 */
	private collectVisibleSymbols(
		symbols: ArcSymbol[],
		position: vscode.Position,
		isSelfCompletion: boolean,
		visibleSymbols: ArcSymbol[],
		inFunctionScope: boolean
	): void {
		for (const symbol of symbols) {
			// Check if the cursor position is within or after this symbol's range
			const isInRange =
				position.line >= symbol.range.start.line &&
				position.line <= symbol.range.end.line;

			if (isSelfCompletion) {
				// Show fields and methods when completing after "self."
				this.handleSelfCompletion(symbol, visibleSymbols);
			} else {
				// Show variables, functions, and function parameters for normal completion
				this.handleNormalCompletion(
					symbol,
					position,
					inFunctionScope,
					visibleSymbols
				);
			}

			// Recurse into children (function bodies, blocks, etc.)
			if (symbol.children && symbol.children.length > 0) {
				if (isInRange || isSelfCompletion) {
					// Inside this scope, so symbols from this scope are visible
					// For self completion, we need to search in class/group children
					// Mark that we're in a function scope so parameters are visible
					const isFunction =
						symbol.kind === SymbolKind.Function ||
						symbol.kind === SymbolKind.SelfFunction;
					this.collectVisibleSymbols(
						symbol.children,
						position,
						isSelfCompletion,
						visibleSymbols,
						isFunction || inFunctionScope
					);
				}
			}
		}
	}

	/**
	 * Handle symbol collection for self completion (self.*)
	 */
	private handleSelfCompletion(
		symbol: ArcSymbol,
		visibleSymbols: ArcSymbol[]
	): void {
		if (
			symbol.kind === SymbolKind.Field ||
			symbol.kind === SymbolKind.SelfFunction
		) {
			visibleSymbols.push(symbol);
		}
	}

	/**
	 * Handle symbol collection for normal completion
	 */
	private handleNormalCompletion(
		symbol: ArcSymbol,
		position: vscode.Position,
		inFunctionScope: boolean,
		visibleSymbols: ArcSymbol[]
	): void {
		if (symbol.kind === SymbolKind.Variable) {
			// Function parameters are always visible within their function scope
			// Other variables must be declared before the cursor position
			if (
				inFunctionScope ||
				this.isDeclaredBeforePosition(symbol, position)
			) {
				visibleSymbols.push(symbol);
			}
		} else if (symbol.kind === SymbolKind.Function) {
			// Functions need to be declared before cursor
			if (this.isDeclaredBeforePosition(symbol, position)) {
				visibleSymbols.push(symbol);
			}
		}
	}

	/**
	 * Check if a symbol is declared before the given position
	 */
	private isDeclaredBeforePosition(
		symbol: ArcSymbol,
		position: vscode.Position
	): boolean {
		return (
			symbol.range.start.line < position.line ||
			(symbol.range.start.line === position.line &&
				symbol.range.start.character < position.character)
		);
	}
}
