import { spawnSync } from "node:child_process";
import { mkdirSync, readFileSync, writeFileSync } from "node:fs";
import { fileURLToPath } from "node:url";
import ts from "typescript";

const grammarDirectory = fileURLToPath(
  new URL("../../src/grammar/", import.meta.url),
);
const outputDirectory = fileURLToPath(
  new URL("../plugins/generated/", import.meta.url),
);
mkdirSync(outputDirectory, { recursive: true });

const result = spawnSync(
  process.execPath,
  [
    fileURLToPath(new URL("../cli/runner.js", import.meta.resolve("antlr-ng"))),
    "-Dlanguage=TypeScript",
    "--generate-listener",
    "false",
    "--generate-visitor",
    "false",
    "-o",
    outputDirectory,
    "ArcSourceCodeLexer.g4",
    "ArcSourceCodeParser.g4",
  ],
  { cwd: grammarDirectory, stdio: "inherit" },
);

if (result.error) throw result.error;
if (result.status !== 0) process.exit(result.status ?? 1);

// Rehype plugins run in Node, outside Next's TypeScript compilation pipeline.
for (const name of ["ArcSourceCodeLexer", "ArcSourceCodeParser"]) {
  const source = readFileSync(`${outputDirectory}${name}.ts`, "utf8");
  const { outputText } = ts.transpileModule(source, {
    compilerOptions: {
      target: ts.ScriptTarget.ES2022,
      module: ts.ModuleKind.ESNext,
    },
  });
  writeFileSync(`${outputDirectory}${name}.js`, outputText);
}
