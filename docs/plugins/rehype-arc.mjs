import {
  BaseErrorListener,
  CharStream,
  CommonTokenStream,
  Token,
} from "antlr4ng";
import { ArcSourceCodeLexer as Lexer } from "./generated/ArcSourceCodeLexer.js";
import { ArcSourceCodeParser as Parser } from "./generated/ArcSourceCodeParser.js";

class ErrorCounter extends BaseErrorListener {
  count = 0;
  syntaxError() {
    this.count++;
  }
}

const primitiveTypes = new Set([
  "KW_INT",
  "KW_DECIMAL",
  "KW_CHAR",
  "KW_STRING",
  "KW_BOOL",
  "KW_BYTE",
  "KW_ANY",
  "KW_INFER",
]);

function identifierKind(node) {
  for (let context = node.parent; context; context = context.parent) {
    switch (Parser.ruleNames[context.ruleIndex]) {
      case "arc_namespace_identifier":
        return "namespace";
      case "arc_data_type":
      case "arc_generic_declaration_wrapper":
      case "arc_group_block":
      case "arc_enum_declarator":
      case "arc_constructor_call":
        return "type";
      case "arc_annotation":
        return "annotation";
      case "arc_function_declarator":
      case "arc_function_call_base":
        return "function";
      case "arc_data_declarator":
        return "variable";
      case "arc_enum_member":
        return "property";
      case "arc_call_chain_term":
        return "variable";
    }
  }
  return "variable";
}

function parseIdentifiers(stream) {
  let best = { errors: Infinity, trees: [] };
  const recoveredNames = new Map();
  function retainConfirmedNames(node) {
    if (node.symbol?.type === Lexer.IDENTIFIER && node.symbol.tokenIndex >= 0) {
      const kind = identifierKind(node);
      for (let context = node.parent; context; context = context.parent) {
        const rule = Parser.ruleNames[context.ruleIndex];
        const marker =
          rule === "arc_function_declarator"
            ? context.KW_FUNCTION()
            : rule === "arc_function_call_base"
              ? context.arc_wrapped_param_list()?.LPAREN()
              : rule === "arc_annotation"
                ? context.AT()
                : null;
        if (
          (kind === "function" || kind === "annotation") &&
          marker?.symbol.tokenIndex >= 0
        ) {
          recoveredNames.set(node.symbol.tokenIndex, kind);
          break;
        }
      }
    }
    for (const child of node.children ?? []) retainConfirmedNames(child);
  }
  // Documentation contains whole programs, declarations, and statement fragments.
  for (const rule of [
    "arc_compilation_unit",
    "arc_namespace_member",
    "arc_group_member",
    "arc_function_declarator",
    "arc_statement",
    "arc_stmt_link",
    "arc_data_type",
    "arc_expression",
  ]) {
    stream.seek(0);
    const errors = new ErrorCounter();
    const trees = [];
    while (stream.LA(1) !== Token.EOF) {
      const parser = new Parser(stream);
      parser.removeErrorListeners();
      parser.addErrorListener(errors);
      const start = stream.index;
      trees.push(parser[rule](0));
      if (stream.index === start) {
        errors.count++;
        stream.consume();
      }
    }
    // Retain real declaration/call markers even when legacy syntax elsewhere recovers poorly.
    for (const tree of trees) retainConfirmedNames(tree);
    if (errors.count < best.errors) best = { errors: errors.count, trees };
    if (errors.count === 0) break;
  }
  const kinds = new Map();
  function visit(node) {
    if (node.symbol?.type === Lexer.IDENTIFIER && node.symbol.tokenIndex >= 0) {
      kinds.set(node.symbol.tokenIndex, identifierKind(node));
    }
    for (const child of node.children ?? []) visit(child);
  }
  for (const tree of best.trees) visit(tree);
  for (const [index, kind] of recoveredNames) kinds.set(index, kind);
  return kinds;
}

export function tokenizeArc(source) {
  // ANTLR offsets count code points rather than JavaScript UTF-16 code units.
  const characters = Array.from(source);
  // The grammar requires a newline to terminate a comment, including at EOF.
  const lexer = new Lexer(CharStream.fromString(`${source}\n`));
  lexer.removeErrorListeners();
  // Retain skipped trivia for rendering, without changing the compiler grammar.
  lexer.skip = function () {
    this.channel = Token.HIDDEN_CHANNEL;
  };
  const stream = new CommonTokenStream(lexer);
  stream.fill();
  const identifiers = parseIdentifiers(stream);
  const result = [];
  let offset = 0;
  for (const token of stream.getTokens()) {
    if (token.type === Token.EOF || token.start >= characters.length) break;
    if (token.start > offset) {
      result.push({
        text: characters.slice(offset, token.start).join(""),
        kind: null,
      });
    }
    const name = Lexer.symbolicNames[token.type];
    let kind = null;
    if (name === "IDENTIFIER")
      kind = identifiers.get(token.tokenIndex) ?? "variable";
    else if (name === "NUMBER") kind = "number";
    else if (name === "LITERAL_STRING") kind = "string";
    else if (name === "LINE_COMMENT") kind = "comment";
    else if (primitiveTypes.has(name)) kind = "type";
    else if (["KW_TRUE", "KW_FALSE", "KW_NONE"].includes(name))
      kind = "constant";
    else if (name?.startsWith("KW_")) kind = "keyword";
    else if (name !== "WHITESPACE") kind = "operator";
    offset = Math.min(token.stop + 1, characters.length);
    result.push({ text: characters.slice(token.start, offset).join(""), kind });
  }
  if (offset < characters.length)
    result.push({ text: characters.slice(offset).join(""), kind: null });
  return result;
}

const textNode = (value) => ({ type: "text", value });

export default function rehypeArc() {
  return function transform(tree) {
    function visit(node) {
      const code = node.tagName === "pre" && node.children?.[0];
      if (
        code?.tagName === "code" &&
        code.properties?.className?.includes("language-arc") &&
        code.children.every((child) => child.type === "text")
      ) {
        const source = code.children.map((child) => child.value).join("");
        code.children = tokenizeArc(source).map(({ text, kind }) =>
          kind
            ? {
                type: "element",
                tagName: "span",
                properties: { className: [`arc-token-${kind}`] },
                children: [textNode(text)],
              }
            : textNode(text),
        );
        node.properties ??= {};
        node.properties.className = [
          ...(node.properties.className ?? []),
          "arc-highlight",
        ];
        return;
      }
      for (const child of node.children ?? []) visit(child);
    }
    visit(tree);
  };
}
