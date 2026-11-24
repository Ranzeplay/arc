import { useEffect, useRef, useState } from "react";
import type {
  HighlighterCore,
  LanguageRegistration,
  ThemeRegistration,
} from "shiki/core";
import "./CodeView.css";

type CodeTheme = "stella-light" | "stella-dark";

const arcLanguage: LanguageRegistration = {
  name: "arc",
  scopeName: "source.arc",
  patterns: [
    { include: "#comments" },
    { include: "#strings" },
    { include: "#annotations" },
    { include: "#constants" },
    { include: "#types" },
    { include: "#keywords" },
    { include: "#numbers" },
    { include: "#operators" },
  ],
  repository: {
    comments: {
      patterns: [
        { name: "comment.line.double-slash.arc", match: "//.*$" },
        {
          name: "comment.block.arc",
          begin: "/\\*",
          end: "\\*/",
        },
      ],
    },
    strings: {
      patterns: [
        { name: "string.quoted.double.arc", match: '"(?:[^"\\\\]|\\\\.)*"' },
        { name: "string.quoted.single.arc", match: "'(?:[^'\\\\]|\\\\.)*'" },
      ],
    },
    annotations: {
      patterns: [{ name: "storage.type.annotation.arc", match: "@\\w+" }],
    },
    constants: {
      patterns: [
        { name: "constant.language.arc", match: "\\b(true|false|none)\\b" },
      ],
    },
    types: {
      patterns: [
        {
          name: "entity.name.type.arc",
          match: "\\b(int|decimal|string|char|bool|byte|any|void)\\b",
        },
      ],
    },
    keywords: {
      patterns: [
        {
          name: "keyword.control.arc",
          match:
            "\\b(namespace|link|public|private|protected|func|var|let|const|return|call|if|elif|else|while|for|foreach|loop|break|continue|group|enum|new|self|mutable|ref)\\b",
        },
      ],
    },
    numbers: {
      patterns: [
        { name: "constant.numeric.arc", match: "\\b\\d+(?:\\.\\d+)?\\b" },
      ],
    },
    operators: {
      patterns: [
        { name: "keyword.operator.arc", match: "[+\\-*/%=<>!&|^~?:]+" },
      ],
    },
  },
};

const themes: ThemeRegistration[] = [
  {
    name: "stella-dark",
    type: "dark",
    colors: {
      "editor.foreground": "#CDD0DB",
      "editor.background": "#111216",
    },
    settings: [
      { settings: { foreground: "#CDD0DB" } },
      { scope: ["comment"], settings: { foreground: "#656977" } },
      { scope: ["keyword"], settings: { foreground: "#C5A2FA" } },
      { scope: ["string"], settings: { foreground: "#A4D9B0" } },
      {
        scope: ["constant.numeric", "constant.language", "storage.type.annotation"],
        settings: { foreground: "#E9BE88" },
      },
      { scope: ["entity.name.type"], settings: { foreground: "#8CCBD3" } },
      { scope: ["keyword.operator"], settings: { foreground: "#B6BAC8" } },
    ],
  },
  {
    name: "stella-light",
    type: "light",
    colors: {
      "editor.foreground": "#303443",
      "editor.background": "#FFFFFF",
    },
    settings: [
      { settings: { foreground: "#303443" } },
      { scope: ["comment"], settings: { foreground: "#727887" } },
      { scope: ["keyword"], settings: { foreground: "#7540A4" } },
      { scope: ["string"], settings: { foreground: "#276B41" } },
      {
        scope: ["constant.numeric", "constant.language", "storage.type.annotation"],
        settings: { foreground: "#95621B" },
      },
      { scope: ["entity.name.type"], settings: { foreground: "#146D80" } },
      { scope: ["keyword.operator"], settings: { foreground: "#53596B" } },
    ],
  },
];

let highlighterPromise: Promise<HighlighterCore> | undefined;
function getHighlighter() {
  highlighterPromise ??= Promise.all([
    import("shiki/core"),
    import("@shikijs/engine-javascript"),
  ]).then(([{ createHighlighterCore }, { createJavaScriptRegexEngine }]) =>
    createHighlighterCore({
      engine: createJavaScriptRegexEngine(),
      langs: [arcLanguage],
      themes,
    }),
  );
  return highlighterPromise;
}

function HighlightedCode({ html, value }: { html: string; value: string }) {
  return html ? (
    <div dangerouslySetInnerHTML={{ __html: html }} />
  ) : (
    <pre className="code-loading">{value}</pre>
  );
}

export function CodeView({
  value,
  theme,
  language = "text",
  wrap = true,
  follow = false,
  revealLine,
  lineKinds,
  onScroll,
  className = "",
}: {
  value: string;
  theme: string;
  language?: "text" | "arc";
  wrap?: boolean;
  follow?: boolean;
  revealLine?: number;
  lineKinds?: ("added" | "removed" | undefined)[];
  onScroll?: (element: HTMLDivElement) => void;
  className?: string;
}) {
  const containerRef = useRef<HTMLDivElement>(null);
  const [html, setHtml] = useState("");
  const codeTheme: CodeTheme =
    theme === "stella-light" ? "stella-light" : "stella-dark";
  const lineKindsKey = lineKinds?.map((kind) => kind || "").join(",");

  useEffect(() => {
    let active = true;
    setHtml("");
    void getHighlighter()
      .then((highlighter) =>
        highlighter.codeToHtml(value, {
          lang: language,
          theme: codeTheme,
          transformers: lineKinds
            ? [
                {
                  line(element, line) {
                    const kind = lineKinds[line - 1];
                    if (kind) this.addClassToHast(element, `diff-line-${kind}`);
                  },
                },
              ]
            : undefined,
        }),
      )
      .then((result) => {
        if (active) setHtml(result);
      })
      .catch(() => {
        if (active) setHtml("");
      });
    return () => {
      active = false;
    };
  }, [value, language, codeTheme, lineKindsKey]);

  useEffect(() => {
    const container = containerRef.current;
    if (!container || !html) return;
    if (follow) container.scrollTop = container.scrollHeight;
    else if (revealLine) {
      container
        .querySelectorAll<HTMLElement>(".line")
        [revealLine - 1]?.scrollIntoView({ block: "center" });
    }
  }, [html, follow, revealLine]);

  return (
    <div
      ref={containerRef}
      className={`code-view ${wrap ? "wrap" : "nowrap"} ${className}`}
      role="region"
      aria-label={language === "arc" ? "Arc source code" : "Output text"}
      tabIndex={0}
      onScroll={(event) => onScroll?.(event.currentTarget)}
    >
      <HighlightedCode html={html} value={value} />
    </div>
  );
}
