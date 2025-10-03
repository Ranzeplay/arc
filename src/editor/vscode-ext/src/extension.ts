import * as vscode from 'vscode';
import { ArcDocumentSymbolProvider } from './providers/documentSymbolProvider';
import { ArcCompletionProvider } from './providers/completionProvider';
import { ArcDiagnosticsProvider } from './providers/diagnosticsProvider';

export function activate(context: vscode.ExtensionContext) {
	console.log('Arc Language Extension is now active!');

	const diagnosticsProvider = new ArcDiagnosticsProvider();
	const documentSymbolProvider = new ArcDocumentSymbolProvider();
	const completionProvider = new ArcCompletionProvider();

	// Register document symbol provider
	const symbolDisposable = vscode.languages.registerDocumentSymbolProvider(
		{ scheme: 'file', language: 'arc' },
		documentSymbolProvider
	);

	// Register completion provider
	const completionDisposable = vscode.languages.registerCompletionItemProvider(
		{ scheme: 'file', language: 'arc' },
		completionProvider,
		'.',
		':',
		'::'
	);

	// Register diagnostics
	const diagnosticsCollection =
		vscode.languages.createDiagnosticCollection('Arc');
	context.subscriptions.push(diagnosticsCollection);

	// Listen for document changes
	const documentChangeDisposable = vscode.workspace.onDidChangeTextDocument(
		async event => {
			await tryRegenerateDocumentInfo(event.document, diagnosticsProvider);
		}
	);

	// Listen for document opens
	const documentOpenDisposable = vscode.workspace.onDidOpenTextDocument(
		async document => {
			await tryRegenerateDocumentInfo(document, diagnosticsProvider);
		}
	);

	context.subscriptions.push(
		symbolDisposable,
		completionDisposable,
		documentChangeDisposable,
		documentOpenDisposable
	);
}

async function tryRegenerateDocumentInfo(
	document: vscode.TextDocument,
	diagnosticsProvider: ArcDiagnosticsProvider
) {
	if (document.languageId === 'arc') {
		diagnosticsProvider.updateDiagnostics(
			document,
			vscode.languages.createDiagnosticCollection('Arc')
		);
	}
}

export function deactivate() {}
