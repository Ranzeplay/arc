import {
  useEffect,
  useRef,
  useState,
  type CSSProperties,
  type PointerEvent,
} from "react";
import {
  ArrowLeftRight,
  Columns2,
  FileCode2,
  Rows2,
  Terminal,
} from "lucide-react";
import { clean, filename, runText } from "@/lib/output";
import { stageNames, type Run } from "@/lib/types";
import { Select } from "./Setup";
import { Button } from "./ui/button";
import { CodeView } from "./CodeView";

type Layout = "columns" | "rows";

export default function Inspector({
  editorTheme,
  sources,
  path,
  setPath,
  source,
  loading,
  runs,
  compare,
  active,
}: {
  editorTheme: string;
  sources: string[];
  path: string;
  setPath: (path: string) => void;
  source: string;
  loading: boolean;
  runs: [Run | undefined, Run | undefined];
  compare: boolean;
  active: 0 | 1;
}) {
  const [runId, setRunId] = useState("");
  const [layout, setLayout] = useState<Layout>("columns");
  const [reversed, setReversed] = useState(false);
  const [split, setSplit] = useState(50);
  const splitRef = useRef<HTMLDivElement>(null);
  const selectedRun = runs.find((run) => run?.id === runId) || runs[active];
  const decodeStage = selectedRun?.stages.decode;
  const decodeOutput = clean(runText(selectedRun, "decode"));

  useEffect(() => {
    try {
      const saved = JSON.parse(
        localStorage.getItem("arc-stella-inspector") || "{}",
      );
      if (saved.layout === "columns" || saved.layout === "rows")
        setLayout(saved.layout);
      if (typeof saved.reversed === "boolean") setReversed(saved.reversed);
      if (typeof saved.split === "number")
        setSplit(Math.min(75, Math.max(25, saved.split)));
    } catch {
      /* Use the default horizontal split if saved layout data is invalid. */
    }
  }, []);

  useEffect(() => {
    try {
      localStorage.setItem(
        "arc-stella-inspector",
        JSON.stringify({ layout, reversed, split }),
      );
    } catch {
      /* Keep the current layout for this session if storage is unavailable. */
    }
  }, [layout, reversed, split]);

  function dragSplit(event: PointerEvent<HTMLDivElement>) {
    const container = splitRef.current;
    if (!container) return;
    event.currentTarget.setPointerCapture(event.pointerId);
    const rect = container.getBoundingClientRect();
    const position =
      layout === "columns"
        ? event.clientX - rect.left
        : event.clientY - rect.top;
    const size = layout === "columns" ? rect.width : rect.height;
    setSplit(Math.min(75, Math.max(25, (position / size) * 100)));
  }

  const sourcePanel = (
    <section className="inspect-panel source-inspect-panel">
      <div className="inspect-panel-heading">
        <span className="panel-title">
          <FileCode2 size={15} /> Source
        </span>
        <Select
          aria-label="Inspector source file"
          value={path}
          onChange={(event) => setPath(event.target.value)}
        >
          {!sources.length && <option value="">Select a source file</option>}
          {[...new Set([...sources, ...(path ? [path] : [])])].map((item) => (
            <option key={item} value={item}>
              {filename(item)}
            </option>
          ))}
        </Select>
        <span className="muted">{loading ? "Loading…" : "Read only"}</span>
      </div>
      <div className="inspect-editor">
        {path ? (
          <CodeView
            value={source}
            language="arc"
            theme={editorTheme}
            wrap
          />
        ) : (
          <div className="source-placeholder">
            <FileCode2 size={22} />
            <span>Choose a source file to inspect.</span>
          </div>
        )}
      </div>
      <div className="inspect-path" title={path}>
        {path || "No source selected"}
      </div>
    </section>
  );

  const decodePanel = (
    <section className="inspect-panel decode-inspect-panel">
      <div className="inspect-panel-heading">
        <span className="panel-title">
          <Terminal size={15} /> cmdec decode
        </span>
        {compare && (
          <Select
            aria-label="Decode result run"
            value={selectedRun?.id || ""}
            onChange={(event) => setRunId(event.target.value)}
          >
            {runs
              .filter((run): run is Run => Boolean(run))
              .map((run) => (
                <option key={run.id} value={run.id}>
                  {run.name} · {run.status}
                </option>
              ))}
          </Select>
        )}
        <span className={`inspect-status ${decodeStage?.status || "idle"}`}>
          {decodeStage?.status || "Ready"}
        </span>
      </div>
      <div className="inspect-metadata">
        <div>
          <span>Package</span>
          <code>
            {selectedRun
              ? `${selectedRun.directory}/build.pkg.arc`
              : "Created after compilation"}
          </code>
        </div>
        <div>
          <span>Decode time</span>
          <code>
            {decodeStage?.durationMs != null
              ? `${(decodeStage.durationMs / 1000).toFixed(2)}s`
              : "—"}
          </code>
        </div>
        <div>
          <span>Exit code</span>
          <code>{decodeStage?.code ?? "—"}</code>
        </div>
        <div className="inspect-command">
          <span>{stageNames.decode} command</span>
          <code>
            {decodeStage?.command ||
              "Run the pipeline to capture the cmdec command."}
          </code>
        </div>
      </div>
      <div className="inspect-editor">
        {decodeOutput ? (
          <CodeView value={decodeOutput} theme={editorTheme} wrap />
        ) : (
          <div className="inspect-empty">
            {decodeStage?.status === "failed"
              ? "The decode stage failed without output."
              : "Decode output appears here after compilation and cmdec parsing."}
          </div>
        )}
      </div>
    </section>
  );

  return (
    <div
      ref={splitRef}
      className={`inspector-split ${layout} ${reversed ? "reversed" : ""}`}
      style={{ "--inspect-split": `${split}%` } as CSSProperties}
    >
      {sourcePanel}
      <div
        className="inspect-divider"
        role="separator"
        aria-orientation={layout === "columns" ? "vertical" : "horizontal"}
        aria-label="Resize source and decode panels"
        onPointerDown={dragSplit}
        onPointerMove={(event) => {
          if (!event.currentTarget.hasPointerCapture(event.pointerId)) return;
          const container = splitRef.current;
          if (!container) return;
          const rect = container.getBoundingClientRect();
          const position =
            layout === "columns"
              ? event.clientX - rect.left
              : event.clientY - rect.top;
          const size = layout === "columns" ? rect.width : rect.height;
          setSplit(Math.min(75, Math.max(25, (position / size) * 100)));
        }}
        onPointerUp={(event) =>
          event.currentTarget.releasePointerCapture(event.pointerId)
        }
      />
      {decodePanel}
      <div className="inspector-toolbar">
        <span>Source and decoded instructions</span>
        <Button
          variant={layout === "columns" ? "secondary" : "ghost"}
          size="icon"
          aria-label="Arrange panels side by side"
          title="Side by side"
          onClick={() => setLayout("columns")}
        >
          <Columns2 />
        </Button>
        <Button
          variant={layout === "rows" ? "secondary" : "ghost"}
          size="icon"
          aria-label="Arrange panels stacked"
          title="Stack panels"
          onClick={() => setLayout("rows")}
        >
          <Rows2 />
        </Button>
        <Button
          variant="ghost"
          size="icon"
          aria-label="Swap source and decode panels"
          title="Swap panels"
          onClick={() => setReversed(!reversed)}
        >
          <ArrowLeftRight />
        </Button>
      </div>
    </div>
  );
}
