import { Fragment, useEffect, useMemo, useRef, useState } from "react";
import { diffLines } from "diff";
import {
  ArrowDownToLine,
  Check,
  Circle,
  CircleAlert,
  Clock3,
  Copy,
  GitCompareArrows,
  Loader2,
  Search,
  Terminal,
  TriangleAlert,
} from "lucide-react";
import { Button } from "./ui/button";
import { Input } from "./ui/input";
import { Tabs, TabsList, TabsTrigger } from "./ui/tabs";
import { Check as CheckField, Select } from "./Setup";
import { clean, diagnostic, filename, runText } from "@/lib/output";
import { CodeView } from "./CodeView";
import {
  stageNames,
  stages,
  type Diagnostic,
  type Run,
  type Stage,
  type Status,
} from "@/lib/types";

export function StatusIcon({ status }: { status: Status }) {
  if (status === "running")
    return <Loader2 size={14} className="animate-spin" />;
  if (status === "passed") return <Check size={14} />;
  if (status === "failed") return <CircleAlert size={14} />;
  return <Circle size={12} />;
}
export default function Output({
  editorTheme,
  runs,
  compare,
  onLocation,
  onError,
}: {
  editorTheme: string;
  runs: [Run | undefined, Run | undefined];
  compare: boolean;
  onLocation: (diagnostic: Diagnostic) => void;
  onError: (message: string) => void;
}) {
  const [stage, setStage] = useState<Stage | "all">("all");
  const [query, setQuery] = useState("");
  const [severity, setSeverity] = useState("all");
  const [stream, setStream] = useState("all");
  const [wrap, setWrap] = useState(false);
  const [follow, setFollow] = useState(true);
  const [diff, setDiff] = useState(true);
  const [normalize, setNormalize] = useState(false);
  const [copied, setCopied] = useState(false);
  const [stageColumns, setStageColumns] = useState<[number, number, number]>(
    () => {
      try {
        const saved = JSON.parse(
          localStorage.getItem("arc-stella-stage-columns") || "null",
        );
        if (
          Array.isArray(saved) &&
          saved.length === 3 &&
          saved.every((value) => typeof value === "number" && value > 0)
        )
          return [saved[0], saved[1], saved[2]];
      } catch {
        /* Use equal stage card widths when saved sizing is unavailable. */
      }
      return [1, 1, 1];
    },
  );
  const stageGridRef = useRef<HTMLDivElement>(null);
  const stageDrag = useRef<{
    boundary: number;
    startX: number;
    gridWidth: number;
    left: number;
    right: number;
  } | null>(null);
  const texts = runs.map((run) =>
    runText(run, stage, stream, query, severity, normalize),
  );
  const allDiagnostics = useMemo(
    () =>
      runs.flatMap((run, index) =>
        compare || index === 0
          ? (run?.events || [])
              .filter(
                (e) =>
                  e.kind === "output" && (stage === "all" || e.stage === stage),
              )
              .map((e) => ({
                ...diagnostic(e.text),
                lane: index,
                stage: e.stage,
              }))
              .filter((d) => d.severity === "error" || d.severity === "warning")
          : [],
      ),
    [runs, stage, compare],
  );
  async function copy() {
    try {
      await navigator.clipboard.writeText(texts[0]);
      setCopied(true);
      setTimeout(() => setCopied(false), 1600);
    } catch (error) {
      onError(String(error));
    }
  }
  const visibleRuns = compare ? runs : [runs[0]];
  const hasRun = visibleRuns.some(Boolean);

  function syncCompareScroll(index: number, source: HTMLDivElement) {
    const panes = source
      .closest(".log-editors")
      ?.querySelectorAll<HTMLDivElement>(".code-view");
    const target = panes?.[1 - index];
    if (target && Math.abs(target.scrollTop - source.scrollTop) > 1)
      target.scrollTop = source.scrollTop;
  }

  useEffect(() => {
    try {
      localStorage.setItem(
        "arc-stella-stage-columns",
        JSON.stringify(stageColumns),
      );
    } catch {
      /* Keep stage card sizing for this session if storage is unavailable. */
    }
  }, [stageColumns]);

  function beginStageResize(
    boundary: number,
    event: React.PointerEvent<HTMLDivElement>,
  ) {
    const grid = stageGridRef.current;
    if (!grid) return;
    event.preventDefault();
    event.currentTarget.setPointerCapture(event.pointerId);
    stageDrag.current = {
      boundary,
      startX: event.clientX,
      gridWidth: grid.getBoundingClientRect().width,
      left: stageColumns[boundary],
      right: stageColumns[boundary + 1],
    };
  }

  function moveStageResize(event: React.PointerEvent<HTMLDivElement>) {
    const drag = stageDrag.current;
    if (!drag || !event.currentTarget.hasPointerCapture(event.pointerId))
      return;
    const delta =
      ((event.clientX - drag.startX) / drag.gridWidth) *
      (drag.left + drag.right);
    const left = Math.min(
      drag.left + drag.right - 0.6,
      Math.max(0.6, drag.left + delta),
    );
    setStageColumns((current) => {
      const next = [...current] as [number, number, number];
      next[drag.boundary] = left;
      next[drag.boundary + 1] = drag.left + drag.right - left;
      return next;
    });
  }

  return (
    <section className="output-panel">
      <div className="output-top">
        <div>
          <h2>{compare ? "Compare runs" : "Pipeline output"}</h2>
        </div>
        <div className="output-top-actions">
          {compare && (
            <>
              <CheckField
                label="Normalize paths & durations"
                checked={normalize}
                onChange={(e) => setNormalize(e.target.checked)}
              />
              <Button
                variant={diff ? "secondary" : "outline"}
                size="sm"
                onClick={() => setDiff(!diff)}
              >
                <GitCompareArrows />
                Diff
              </Button>
            </>
          )}
          <Button variant="outline" size="sm" onClick={copy} disabled={!hasRun}>
            {copied ? <Check /> : <Copy />}
            {copied ? "Copied" : "Copy output"}
          </Button>
        </div>
      </div>
      <div
        ref={stageGridRef}
        className="stage-cards"
        style={{
          gridTemplateColumns: `${stageColumns[0]}fr 8px ${stageColumns[1]}fr 8px ${stageColumns[2]}fr`,
        }}
      >
        {stages.map((key, index) => (
          <Fragment key={key}>
            <button
              className={`stage-card ${stage === key ? "selected" : ""}`}
              style={{ gridColumn: index * 2 + 1 }}
              onClick={() => setStage(stage === key ? "all" : key)}
            >
              <span className="stage-card-title">{stageNames[key]}</span>
              <span className="stage-results">
                {visibleRuns.map((run, index) => (
                  <span
                    key={index}
                    className={`stage-result ${run?.stages[key].status || "idle"}`}
                  >
                    {compare && <b>{index === 0 ? "A" : "B"}</b>}
                    <StatusIcon status={run?.stages[key].status || "idle"} />
                    <span>{run?.stages[key].status || "Ready"}</span>
                    {run?.stages[key].durationMs != null && (
                      <small>
                        {(run.stages[key].durationMs! / 1000).toFixed(2)}s
                      </small>
                    )}
                  </span>
                ))}
              </span>
            </button>
            {index < stages.length - 1 && (
              <div
                className="stage-resizer"
                role="separator"
                aria-orientation="vertical"
                aria-label={`Resize ${stageNames[key]} and ${stageNames[stages[index + 1]]} cards`}
                style={{ gridColumn: index * 2 + 2 }}
                onPointerDown={(event) => beginStageResize(index, event)}
                onPointerMove={moveStageResize}
                onPointerUp={(event) => {
                  event.currentTarget.releasePointerCapture(event.pointerId);
                  stageDrag.current = null;
                }}
              />
            )}
          </Fragment>
        ))}
      </div>
      <div className="log-toolbar">
        <Tabs
          value={stage}
          onValueChange={(value) => setStage(value as Stage | "all")}
        >
          <TabsList>
            <TabsTrigger value="all">All messages</TabsTrigger>
            {stages.map((key) => (
              <TabsTrigger key={key} value={key}>
                {stageNames[key]}
              </TabsTrigger>
            ))}
          </TabsList>
        </Tabs>
        <div className="log-toggles">
          <Button
            variant={wrap ? "secondary" : "ghost"}
            size="sm"
            onClick={() => setWrap(!wrap)}
            title="Toggle line wrapping"
          >
            Wrap
          </Button>
          <Button
            variant={follow ? "secondary" : "ghost"}
            size="sm"
            onClick={() => setFollow(!follow)}
            title="Toggle follow output"
          >
            <ArrowDownToLine />
            Follow
          </Button>
        </div>
      </div>
      <div className="filter-toolbar">
        <div className="search-field">
          <Search size={14} />
          <Input
            aria-label="Filter output"
            placeholder="Search output…"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
        </div>
        <Select
          aria-label="Filter severity"
          value={severity}
          onChange={(e) => setSeverity(e.target.value)}
        >
          <option value="all">All levels</option>
          {["error", "warning", "info", "debug"].map((level) => (
            <option key={level} value={level}>
              {level}
            </option>
          ))}
        </Select>
        <Select
          aria-label="Filter stream"
          value={stream}
          onChange={(e) => setStream(e.target.value)}
        >
          <option value="all">Both streams</option>
          <option>stdout</option>
          <option>stderr</option>
        </Select>
        <span className="message-count">
          {visibleRuns.reduce(
            (n, run) =>
              n + (run?.events.filter((e) => e.kind === "output").length || 0),
            0,
          )}{" "}
          messages
        </span>
      </div>
      <div className="run-headings">
        {visibleRuns.map((run, index) => (
          <div key={index} className="run-heading">
            <span className={`lane-tag lane-${index}`}>
              {index === 0 ? "A" : "B"}
            </span>
            <strong>
              {run?.name || (index === 0 ? "Baseline" : "Candidate")}
            </strong>
            <span className={`status-pill ${run?.status || "idle"}`}>
              <StatusIcon status={run?.status || "idle"} />
              {run?.status || "Ready"}
            </span>
            {run && (
              <span className="run-time">
                <Clock3 size={12} />
                {new Date(run.started).toLocaleTimeString([], {
                  hour: "2-digit",
                  minute: "2-digit",
                })}
              </span>
            )}
          </div>
        ))}
      </div>
      {hasRun && (
        <div className="command-rows">
          {visibleRuns.map((run, index) => (
            <div key={index}>
              {(stage === "all" ? stages : [stage])
                .filter(
                  (key) => run?.stages[key].command || run?.stages[key].message,
                )
                .map((key) => (
                  <details key={key}>
                    <summary>
                      <Terminal size={12} />
                      <span>{stageNames[key]}</span>
                      <span className="command-code">
                        {run?.stages[key].code != null
                          ? `Exit ${run.stages[key].code}`
                          : run?.stages[key].status}
                      </span>
                    </summary>
                    <pre>{run?.stages[key].command}</pre>
                    {run?.stages[key].message && (
                      <p>{run.stages[key].message}</p>
                    )}
                  </details>
                ))}
            </div>
          ))}
        </div>
      )}
      <div className="logs-body">
        {!hasRun ? (
          <div className="empty-state">
            <div className="empty-terminal">
              <Terminal size={26} />
            </div>
            <h3>Run the pipeline</h3>
            <p>
              Select your source and tools, then run the pipeline.
              <br />
              Build messages, diagnostics, and program output appear here.
            </p>
            <div className="empty-flow">
              <span>Compile</span>
              <span>→</span>
              <span>Decode</span>
              <span>→</span>
              <span>Execute</span>
            </div>
          </div>
        ) : compare && diff ? (
          <div className="log-editors comparison diff-comparison">
            {(() => {
              const chunks = diffLines(texts[0], texts[1]);
              const kinds: ("added" | "removed" | undefined)[][] = [[], []];
              for (const chunk of chunks) {
                const lineCount =
                  chunk.count ??
                  chunk.value.split("\n").length -
                    (chunk.value.endsWith("\n") ? 1 : 0);
                const lane = chunk.removed ? 0 : chunk.added ? 1 : -1;
                if (lane >= 0)
                  for (let index = 0; index < lineCount; index++)
                    kinds[lane].push(chunk.removed ? "removed" : "added");
                else {
                  kinds[0].push(...Array(lineCount).fill(undefined));
                  kinds[1].push(...Array(lineCount).fill(undefined));
                }
              }
              return texts.map((text, index) => (
                <div key={index} className="log-editor">
                  <CodeView
                    value={text}
                    theme={editorTheme}
                    wrap={wrap}
                    follow={follow}
                    lineKinds={kinds[index]}
                    onScroll={(element) => syncCompareScroll(index, element)}
                    className="output-code"
                  />
                </div>
              ));
            })()}
          </div>
        ) : (
          <div className={`log-editors ${compare ? "comparison" : ""}`}>
            {visibleRuns.map((run, index) => (
              <div key={index} className="log-editor">
                {run ? (
                  <CodeView
                    value={texts[index]}
                    theme={editorTheme}
                    wrap={wrap}
                    follow={follow}
                    onScroll={(element) => syncCompareScroll(index, element)}
                    className="output-code"
                  />
                ) : (
                  <div className="waiting-state">
                    Run {index === 0 ? "A" : "B"} to compare output.
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
      </div>
      {visibleRuns.some((run) => run?.truncated) && (
        <p className="truncation">
          Showing the latest 10,000 events. Full output is stored in the run
          directory.
        </p>
      )}
      {allDiagnostics.length > 0 && (
        <details className="diagnostics" open>
          <summary>
            <TriangleAlert size={14} />
            Diagnostics<span>{allDiagnostics.length}</span>
          </summary>
          <div className="diagnostic-list">
            {allDiagnostics.slice(-100).map((d, index) => (
              <button
                key={index}
                className={`diagnostic ${d.severity}`}
                disabled={!d.path}
                onClick={() => onLocation(d)}
              >
                <span className="diagnostic-icon">
                  {d.severity === "error" ? (
                    <CircleAlert size={14} />
                  ) : (
                    <TriangleAlert size={14} />
                  )}
                </span>
                {compare && (
                  <span className={`lane-tag lane-${d.lane}`}>
                    {d.lane === 0 ? "A" : "B"}
                  </span>
                )}
                <span className="diagnostic-message">{d.message}</span>
                {d.path && (
                  <span className="diagnostic-location">
                    {filename(d.path)}:{d.line}
                  </span>
                )}
              </button>
            ))}
          </div>
        </details>
      )}
      {visibleRuns.some((run) => run?.status === "failed") && (
        <div className="run-error">
          {clean(
            visibleRuns
              .find((run) => run?.status === "failed")
              ?.events.findLast((e) => e.kind === "finished")?.text ||
              "A stage failed. Inspect its output above.",
          )}
        </div>
      )}
    </section>
  );
}
