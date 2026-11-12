import assert from "node:assert/strict";
import { readdirSync, readFileSync } from "node:fs";
import test from "node:test";
import { compile } from "@mdx-js/mdx";
import rehypeShiki from "@shikijs/rehype";
import rehypeArc, { tokenizeArc } from "../plugins/rehype-arc.mjs";

test("grammar tokens and parser contexts classify Arc programs", () => {
  const source = `link Std::Console;
namespace Example {
  @Entry public func main(var args: string[]): int {
    var message: string = "Hello 🌏";
    call PrintString(message);
    return 0x2a;
  }
}`;
  const tokens = tokenizeArc(source);
  const has = (text, kind) =>
    assert.ok(
      tokens.some((token) => token.text === text && token.kind === kind),
      `${text}: ${kind}`,
    );
  assert.equal(tokens.map((token) => token.text).join(""), source);
  has("namespace", "keyword");
  has("Example", "namespace");
  has("Entry", "annotation");
  has("main", "function");
  has("PrintString", "function");
  has("args", "variable");
  has("string", "type");
  has('"Hello 🌏"', "string");
  has("0x2a", "number");
});

test("partial declarations, statements, and legacy syntax remain lossless", () => {
  for (const source of [
    "public func PrintString(const s: string): none",
    "public group Box<T> { public field var item: T; }",
    "call item.update(42);\ncall [Example::Item].update(item, 42);",
    "<accessibility> func <name>([parameters]): <return type>",
    "var foo: val int;\nconst bar: ref string;",
    "# final comment 🌏",
    "# a comment\r\nvar x: int = -1.2e+3;\r\n",
    'var bad: int = $;\n"unfinished',
    "",
  ]) {
    assert.equal(
      tokenizeArc(source)
        .map((token) => token.text)
        .join(""),
      source,
    );
  }
  assert.equal(tokenizeArc("# final comment")[0].kind, "comment");
  assert.ok(
    tokenizeArc("public func PrintString(const s: string): none").some(
      (token) => token.text === "PrintString" && token.kind === "function",
    ),
  );
  assert.ok(
    tokenizeArc(
      'namespace Example { public func main(var args: val string[]): val int { call PrintString("Hi"); return 0; } }',
    ).some((token) => token.text === "main" && token.kind === "function"),
  );
});

test("MDX pipeline highlights Arc and retains other language highlighting", async () => {
  const result = String(
    await compile(
      '```arc\ncall Print("<>&");\n```\n\n```bash\necho hello\n```',
      {
        rehypePlugins: [rehypeArc, [rehypeShiki, { theme: "vitesse-light" }]],
      },
    ),
  );
  assert.match(result, /arc-highlight/);
  assert.match(result, /arc-token-function/);
  assert.match(result, /shiki/);
});

test("all documented Arc fences can be highlighted without losing source", () => {
  const root = new URL("../app/content/", import.meta.url);
  let count = 0;
  for (const file of readdirSync(root, { recursive: true })) {
    if (!file.endsWith(".mdx")) continue;
    const source = readFileSync(new URL(file, root), "utf8");
    for (const match of source.matchAll(/^```arc[^\n]*\n([\s\S]*?)^```/gm)) {
      assert.equal(
        tokenizeArc(match[1])
          .map((token) => token.text)
          .join(""),
        match[1],
        file,
      );
      count++;
    }
  }
  assert.ok(count > 20, `Only ${count} Arc fences found`);
});
