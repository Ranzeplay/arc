import * as vscode from 'vscode';
import { ArcParser, Symbol, SymbolKind } from '../parser/ArcParser';

export class ArcDocumentSymbolProvider implements vscode.DocumentSymbolProvider {
    private parser = new ArcParser();

    provideDocumentSymbols(
        document: vscode.TextDocument,
        token: vscode.CancellationToken
    ): vscode.ProviderResult<vscode.SymbolInformation[] | vscode.DocumentSymbol[]> {
        const result = this.parser.parse(document.getText());

        return result.symbols.map(symbol => this.convertToVSCodeSymbol(symbol, document));
    }

    private convertToVSCodeSymbol(symbol: Symbol, document: vscode.TextDocument): vscode.DocumentSymbol {
        const range = new vscode.Range(
            new vscode.Position(symbol.range.start.line, symbol.range.start.character),
            new vscode.Position(symbol.range.end.line, symbol.range.end.character)
        );

        const kind = this.mapSymbolKind(symbol.kind);
        
        const docSymbol = new vscode.DocumentSymbol(
            symbol.name,
            '',
            kind,
            range,
            range
        );

        if (symbol.children) {
            docSymbol.children = symbol.children.map(child => this.convertToVSCodeSymbol(child, document));
        }

        return docSymbol;
    }

    private mapSymbolKind(kind: SymbolKind): vscode.SymbolKind {
        switch (kind) {
            case SymbolKind.Function:
                return vscode.SymbolKind.Function;
            case SymbolKind.Variable:
                return vscode.SymbolKind.Variable;
            case SymbolKind.Class:
                return vscode.SymbolKind.Class;
            case SymbolKind.Namespace:
                return vscode.SymbolKind.Namespace;
            case SymbolKind.Enum:
                return vscode.SymbolKind.Enum;
            case SymbolKind.Field:
                return vscode.SymbolKind.Field;
            case SymbolKind.SelfFunction:
                return vscode.SymbolKind.Method;
            default:
                return vscode.SymbolKind.Variable;
        }
    }
}
