import * as vscode from 'vscode';
import { ArcParser } from '../parser/ArcParser';

export class ArcDiagnosticsProvider {
	private parser = new ArcParser();

	updateDiagnostics(
		document: vscode.TextDocument,
		collection: vscode.DiagnosticCollection
	): void {
		const result = this.parser.parse(document.getText());

		const diagnostics: vscode.Diagnostic[] = result.errors.map(error => {
			const range = new vscode.Range(
				new vscode.Position(error.line, error.column),
				new vscode.Position(error.line, error.column + error.length)
			);

			const diagnostic = new vscode.Diagnostic(
				range,
				error.message,
				vscode.DiagnosticSeverity.Error
			);

			diagnostic.source = 'Arc Parser';
			return diagnostic;
		});

		collection.set(document.uri, diagnostics);
	}
}
