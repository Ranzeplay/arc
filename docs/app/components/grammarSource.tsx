import fs from "node:fs";
import path from "node:path";

type GrammarFile = "ArcSourceCodeLexer.g4" | "ArcSourceCodeParser.g4";

export default function GrammarSource({ file }: { file: GrammarFile }) {
  const grammarPath = path.join(process.cwd(), "..", "src", "grammar", file);
  const source = fs.readFileSync(grammarPath, "utf8");

  return (
    <pre className="overflow-auto rounded-md bg-neutral-100 p-4 text-sm dark:bg-neutral-900">
      <code className="text-black dark:text-white">{source}</code>
    </pre>
  );
}
