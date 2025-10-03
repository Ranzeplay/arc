import { ANTLRInputStream, CommonTokenStream } from 'antlr4ts';
import { ParseTreeWalker } from 'antlr4ts/tree/ParseTreeWalker';
import { ArcSourceCodeLexer } from './ArcSourceCodeLexer';
import { Arc_function_blockContext, Arc_namespace_blockContext, Arc_statementContext, Arc_stmt_declContext, ArcSourceCodeParser } from './ArcSourceCodeParser';
import { ArcSourceCodeParserListener } from './ArcSourceCodeParserListener';

export interface ParseResult {
    tree: any;
    errors: ParseError[];
    symbols: Symbol[];
}

export interface ParseError {
    line: number;
    column: number;
    message: string;
    length: number;
}

export interface Symbol {
    name: string;
    kind: SymbolKind;
    range: {
        start: { line: number; character: number };
        end: { line: number; character: number };
    };
    children?: Symbol[];
}

export enum SymbolKind {
    Function = 'function',
    SelfFunction = 'selffunction',
    Variable = 'variable',
    Class = 'class',
    Namespace = 'namespace',
    Enum = 'enum',
    Field = 'field'
}

export class ArcParser {
    parse(code: string): ParseResult {
        const inputStream = new ANTLRInputStream(code);
        const lexer = new ArcSourceCodeLexer(inputStream);
        const tokenStream = new CommonTokenStream(lexer);
        const parser = new ArcSourceCodeParser(tokenStream);

        // Remove default error listeners
        parser.removeErrorListeners();
        lexer.removeErrorListeners();

        const errors: ParseError[] = [];
        const symbols: Symbol[] = [];

        // Add custom error listener
        parser.addErrorListener({
            syntaxError: (recognizer, offendingSymbol, line, charPositionInLine, msg, e) => {
                errors.push({
                    line: line - 1,
                    column: charPositionInLine,
                    message: msg,
                    length: 1
                });
            }
        });

        const tree = parser.arc_compilation_unit();

        // Extract symbols using visitor pattern
        const symbolExtractor = new SymbolExtractor();
        ParseTreeWalker.DEFAULT.walk(symbolExtractor, tree);
        symbols.push(...symbolExtractor.getSymbols());

        return {
            tree,
            errors,
            symbols
        };
    }
}

class SymbolExtractor implements ArcSourceCodeParserListener {
    private symbols: Symbol[] = [];

    getSymbols(): Symbol[] {
        return this.symbols;
    }

    enterArc_namespace_block(ctx: Arc_namespace_blockContext) {
        const fullName = ctx.arc_namespace_declarator().arc_namespace_identifier().text;

        const enums: Symbol[] = [];
        const functions: Symbol[] = [];
        const groups: Symbol[] = [];

        for (const member of ctx.arc_namespace_member()) {
            if (member.arc_enum_declarator()) {
                const enumCtx = member.arc_enum_declarator()!;
                if (enumCtx.arc_single_identifier()) {
                    const name = enumCtx.arc_single_identifier().text;
                    const start = enumCtx.start;
                    const stop = enumCtx.stop;

                    const members: Symbol[] = [];
                    for (const enumMember of enumCtx.arc_enum_member()) {
                        if (enumMember.arc_single_identifier()) {
                            const memberName = enumMember.arc_single_identifier().text;
                            const memberStart = enumMember.start;
                            const memberStop = enumMember.stop;

                            members.push({
                                name: memberName,
                                kind: SymbolKind.Field,
                                range: {
                                    start: { line: memberStart.line - 1, character: memberStart.charPositionInLine },
                                    end: { line: memberStop!.line - 1, character: memberStop!.charPositionInLine + memberStop!.text!.length }
                                }
                            });
                        }
                    }

                    enums.push({
                        name,
                        kind: SymbolKind.Enum,
                        range: {
                            start: { line: start.line - 1, character: start.charPositionInLine },
                            end: { line: stop!.line - 1, character: stop!.charPositionInLine + stop!.text!.length }
                        },
                        children: members
                    });
                }
            } else if (member.arc_function_block()) {
                const funcCtx = member.arc_function_block()!;
                const funcSymbol = parseFunction(funcCtx);
                if (funcSymbol) {
                    functions.push(funcSymbol);
                }
            } else if (member.arc_group_block()) {
                const groupCtx = member.arc_group_block()!;
                if (groupCtx.arc_single_identifier()) {
                    const name = groupCtx.arc_single_identifier().text;
                    const start = groupCtx.start;
                    const stop = groupCtx.stop;

                    const fields: Symbol[] = [];
                    const functions: Symbol[] = [];
                    const lifecycleFunctions: Symbol[] = [];

                    for (const members of groupCtx.arc_wrapped_group_member().arc_group_member()) {
                        if (members.arc_group_function()?.arc_function_block()) {
                            const funcCtx = members.arc_group_function()!.arc_function_block()!;
                            const funcSymbol = parseFunction(funcCtx);
                            if (funcSymbol) {
                                if(funcCtx.arc_function_declarator()?.arc_wrapped_arg_list()?.arc_arg_list()?.arc_self_data_declarator()) {
                                    funcSymbol.kind = SymbolKind.SelfFunction;
                                }

                                functions.push(funcSymbol);
                            }
                        } else if (members.arc_group_field()) {
                            const fieldCtx = members.arc_group_field()!;
                            if (fieldCtx.arc_data_declarator() && fieldCtx.arc_data_declarator().arc_single_identifier()) {
                                const fieldName = fieldCtx.arc_data_declarator().arc_single_identifier().text;
                                const fieldStart = fieldCtx.start;
                                const fieldStop = fieldCtx.stop;

                                fields.push({
                                    name: fieldName,
                                    kind: SymbolKind.Field,
                                    range: {
                                        start: { line: fieldStart.line - 1, character: fieldStart.charPositionInLine },
                                        end: { line: fieldStop!.line - 1, character: fieldStop!.charPositionInLine + fieldStop!.text!.length }
                                    }
                                });
                            }
                        } else if (members.arc_group_lifecycle_function()) {
                            const lifeCtx = members.arc_group_lifecycle_function()!;

                            const lifeName = lifeCtx.arc_group_lifecycle_keyword().text;
                            const lifeStart = lifeCtx.start;
                            const lifeStop = lifeCtx.arc_data_type().stop;

                            lifecycleFunctions.push({
                                name: lifeName,
                                kind: SymbolKind.Function,
                                range: {
                                    start: { line: lifeStart.line - 1, character: lifeStart.charPositionInLine },
                                    end: { line: lifeStop!.line - 1, character: lifeStop!.charPositionInLine + lifeStop!.text!.length }
                                }
                            });
                        }
                    }

                    groups.push({
                        name,
                        kind: SymbolKind.Class,
                        range: {
                            start: { line: start.line - 1, character: start.charPositionInLine },
                            end: { line: stop!.line - 1, character: stop!.charPositionInLine + stop!.text!.length }
                        },
                        children: [...fields, ...functions, ...lifecycleFunctions]
                    });
                }
            }
        }

        this.symbols.push({
            name: fullName,
            kind: SymbolKind.Namespace,
            range: {
                start: { line: ctx.start.line - 1, character: ctx.start.charPositionInLine },
                end: { line: ctx.stop!.line - 1, character: ctx.stop!.charPositionInLine + ctx.stop!.text!.length }
            },
            children: [...enums, ...functions, ...groups]
        });
    }

    // Implement other required methods with empty bodies
    enterEveryRule() { }
    exitEveryRule() { }
    visitTerminal() { }
    visitErrorNode() { }
}

function parseFunction(ctx: Arc_function_blockContext): Symbol | null {
    if (ctx.arc_function_declarator()) {
        const funcDeclCtx = ctx.arc_function_declarator()!;
        const name = funcDeclCtx.arc_single_identifier().text;
        const start = funcDeclCtx.start;
        const stop = funcDeclCtx.stop;

        // Extract arguments
        const funcArgs: Symbol[] = [];
        if (funcDeclCtx.arc_wrapped_arg_list()?.arc_arg_list()?.arc_data_declarator()) {
            for (const param of funcDeclCtx.arc_wrapped_arg_list()!.arc_arg_list()!.arc_data_declarator()!) {
                const paramName = param.arc_single_identifier().text;
                const paramStart = param.start;
                const paramStop = param.stop;

                funcArgs.push({
                    name: paramName,
                    kind: SymbolKind.Variable,
                    range: {
                        start: { line: paramStart.line - 1, character: paramStart.charPositionInLine },
                        end: { line: paramStop!.line - 1, character: paramStop!.charPositionInLine + paramStop!.text!.length }
                    }
                });

            }
        }

        // Retrieve inner variables and constants
        let innerSymbols = parseStatements(ctx.arc_wrapped_function_body().arc_statement());

        return {
            name,
            kind: SymbolKind.Function,
            range: {
                start: { line: start.line - 1, character: start.charPositionInLine },
                end: { line: stop!.line - 1, character: stop!.charPositionInLine + stop!.text!.length }
            },
            children: [...funcArgs, ...innerSymbols]
        };
    }

    return null;
}

function parseStatements(ctx: Arc_statementContext[]): Symbol[] {
    const symbols: Symbol[] = [];

    for (const stmt of ctx) {
        if (stmt.arc_stmt_decl()) {
            const declCtx = stmt.arc_stmt_decl()!;
            const symbol = parseDataDeclarator(declCtx);
            if (symbol) {
                symbols.push(symbol);
            }
        } else if (stmt.arc_stmt_while()) {
            const whileCtx = stmt.arc_stmt_while()!;
            const innerSymbols = parseStatements(whileCtx.arc_wrapped_function_body().arc_statement());
            symbols.push(...innerSymbols);
        } else if (stmt.arc_stmt_for()) {
            const forCtx = stmt.arc_stmt_for()!;
            const leadingSymbol = parseDataDeclarator(forCtx.arc_stmt_decl()!);

            const innerSymbols = parseStatements(forCtx.arc_wrapped_function_body().arc_statement());
            if (leadingSymbol) {
                leadingSymbol.children = innerSymbols;
                symbols.push(leadingSymbol);
            }
        } else if (stmt.arc_stmt_if()) {
            const ifCtx = stmt.arc_stmt_if()!;
            const innerSymbols = parseStatements(ifCtx.arc_wrapped_function_body().flatMap(b => b.arc_statement()));
            symbols.push(...innerSymbols);
        }
    }

    return symbols;
}

function parseDataDeclarator(ctx: Arc_stmt_declContext): Symbol | null {
    if (ctx.arc_data_declarator() && ctx.arc_data_declarator().arc_single_identifier()) {
        const name = ctx.arc_data_declarator().arc_single_identifier().text;
        const start = ctx.start;
        const stop = ctx.stop;

        return {
            name,
            kind: SymbolKind.Variable,
            range: {
                start: { line: start.line - 1, character: start.charPositionInLine },
                end: { line: stop!.line - 1, character: stop!.charPositionInLine + stop!.text!.length }
            }
        };
    }
    return null;
}

