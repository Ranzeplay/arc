import { ANTLRInputStream, CommonTokenStream } from 'antlr4ts';
import { ParseTreeWalker } from 'antlr4ts/tree/ParseTreeWalker';
import { ArcSourceCodeLexer } from './ArcSourceCodeLexer';
import { Arc_enum_declaratorContext, Arc_function_declaratorContext, Arc_group_blockContext, Arc_namespace_blockContext, Arc_namespace_declaratorContext, Arc_namespace_memberContext, Arc_stmt_declContext, ArcSourceCodeParser } from './ArcSourceCodeParser';
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
                if (funcCtx.arc_function_declarator()) {
                    const funcDeclCtx = funcCtx.arc_function_declarator()!;
                    const name = funcDeclCtx.arc_single_identifier().text;
                    const start = funcDeclCtx.start;
                    const stop = funcDeclCtx.stop;

                    functions.push({
                        name,
                        kind: SymbolKind.Function,
                        range: {
                            start: { line: start.line - 1, character: start.charPositionInLine },
                            end: { line: stop!.line - 1, character: stop!.charPositionInLine + stop!.text!.length }
                        }
                    });
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

                    // TODO: Extract fields and functions from groupCtx if needed

                    for(const members of groupCtx.arc_wrapped_group_member().arc_group_member()) {
                        if(members.arc_group_function()?.arc_function_block()) {
                            const funcCtx = members.arc_group_function()!.arc_function_block()!;
                            if (funcCtx.arc_function_declarator()) {
                                const funcDeclCtx = funcCtx.arc_function_declarator()!;
                                const funcName = funcDeclCtx.arc_single_identifier().text;
                                const funcStart = funcDeclCtx.start;
                                const funcStop = funcDeclCtx.stop;
                                
                                functions.push({
                                    name: funcName,
                                    kind: SymbolKind.Function,
                                    range: {
                                        start: { line: funcStart.line - 1, character: funcStart.charPositionInLine },
                                        end: { line: funcStop!.line - 1, character: funcStop!.charPositionInLine + funcStop!.text!.length }
                                    }
                                });
                            }
                        } else if(members.arc_group_field()) {
                            const fieldCtx = members.arc_group_field()!;
                            if(fieldCtx.arc_data_declarator() && fieldCtx.arc_data_declarator().arc_single_identifier()) {
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
                        } else if(members.arc_group_lifecycle_function()) {
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
    enterEveryRule() {}
    exitEveryRule() {}
    visitTerminal() {}
    visitErrorNode() {}
}
