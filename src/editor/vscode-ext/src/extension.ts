import * as vscode from "vscode";
import { ArcCompletionProvider } from "./providers/completionProvider";
import { ArcDiagnosticsProvider } from "./providers/diagnosticsProvider";
import { ArcDocumentSymbolProvider } from "./providers/documentSymbolProvider";

export function activate(context: vscode.ExtensionContext) {
	console.log("Arc Language Extension is now active!");

	const diagnosticsProvider = new ArcDiagnosticsProvider();
	const documentSymbolProvider = new ArcDocumentSymbolProvider();
	const completionProvider = new ArcCompletionProvider();

	// Register document symbol provider
	const symbolDisposable = vscode.languages.registerDocumentSymbolProvider(
		{ scheme: "file", language: "arc" },
		documentSymbolProvider,
	);

	// Register completion provider
	const completionDisposable = vscode.languages.registerCompletionItemProvider(
		{ scheme: "file", language: "arc" },
		completionProvider,
		".",
		":",
		"::",
	);

	// Register diagnostics
	const diagnosticsCollection =
		vscode.languages.createDiagnosticCollection("Arc");
	context.subscriptions.push(diagnosticsCollection);

	// Debounce timer for document changes
	let debounceTimer: NodeJS.Timeout | undefined;
	const debounceDelay = 500; // 500ms delay

	// Listen for document changes
	const documentChangeDisposable = vscode.workspace.onDidChangeTextDocument(
		async (event) => {
			if (debounceTimer) {
				clearTimeout(debounceTimer);
			}
			debounceTimer = setTimeout(() => {
				tryRegenerateDocumentInfo(
					event.document,
					diagnosticsProvider,
					diagnosticsCollection,
				);
			}, debounceDelay);
		},
	);

	// Listen for document opens
	const documentOpenDisposable = vscode.workspace.onDidOpenTextDocument(
		async (document) => {
			await tryRegenerateDocumentInfo(
				document,
				diagnosticsProvider,
				diagnosticsCollection,
			);
		},
	);

	context.subscriptions.push(
		symbolDisposable,
		completionDisposable,
		documentChangeDisposable,
		documentOpenDisposable,
	);
}

async function tryRegenerateDocumentInfo(
	document: vscode.TextDocument,
	diagnosticsProvider: ArcDiagnosticsProvider,
	diagnosticsCollection: vscode.DiagnosticCollection,
) {
	if (document.languageId === "arc") {
		diagnosticsProvider.updateDiagnostics(document, diagnosticsCollection);
	}
}

export function deactivate() {}
