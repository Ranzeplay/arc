import type { Diagnostic, Run, RunEvent, Stage } from "./types";

export function clean(text: string) {
  return text.replace(/\x1b\[[0-?]*[ -/]*[@-~]/g, "").replace(/\r/g, "");
}
export function diagnostic(text: string): Diagnostic {
  const message = clean(text).trimEnd();
  const lower = message.toLowerCase();
  const prefix = lower.match(
    /^\s*(?:\[([ewdt*])\]|(fail|crit|error|warn(?:ing)?|info|debug|trace)\b)/,
  );
  const level = prefix?.[1] || prefix?.[2];
  const severity: Diagnostic["severity"] = level
    ? ["e", "fail", "crit", "error"].includes(level)
      ? "error"
      : ["w", "warn", "warning"].includes(level)
        ? "warning"
        : ["d", "t", "debug", "trace"].includes(level)
          ? "debug"
          : "info"
    : /\b(error|critical|fail(?:ed)?|panic)\b/.test(lower)
      ? "error"
      : /\b(warn(?:ing)?)\b/.test(lower)
        ? "warning"
        : /\b(debug|trace)\b/.test(lower)
          ? "debug"
          : "info";
  // Absolute paths can contain spaces. Compiler console prefixes end before the path.
  const spaced = message.match(
    /((?:[A-Za-z]:[\\/]|\/)[^\n]+?\.arc)\s+(?:\[\((\d+),(\d+)\)~|\((\d+),(\d+)\):)/,
  );
  if (spaced)
    return {
      severity,
      message,
      path: spaced[1],
      line: +(spaced[2] || spaced[4]),
      column: spaced[2] ? +spaced[3] + 1 : +spaced[5],
    };
  const loc = message.match(
    /(?:^|\s)((?:[A-Za-z]:)?[^\s]*?\.arc)\s+\[\((\d+),(\d+)\)~/,
  );
  if (loc)
    return {
      severity,
      message,
      path: loc[1],
      line: +loc[2],
      column: +loc[3] + 1,
    };
  const syntax = message.match(
    /(?:^|\s)((?:[A-Za-z]:)?[^\s]*?\.arc)\s+\((\d+),(\d+)\):/,
  );
  if (syntax)
    return {
      severity,
      message,
      path: syntax[1],
      line: +syntax[2],
      column: +syntax[3],
    };
  return { severity, message };
}
export function reduceEvent(run: Run, event: RunEvent): Run {
  const next = {
    ...run,
    stages: { ...run.stages },
    events: [...run.events, event],
  };
  if (next.events.length > 10000) {
    next.events = next.events.slice(-10000);
    next.truncated = true;
  }
  if (event.kind === "finished") {
    next.status = event.status;
    if (event.status === "failed") {
      for (const key of ["compile", "decode", "execute"] as Stage[]) {
        if (next.stages[key].status === "running")
          next.stages[key] = {
            ...next.stages[key],
            status: "failed",
            message: event.text,
          };
      }
    }
  }
  if (event.kind === "stage" && event.stage !== "all") {
    const previous = next.stages[event.stage];
    next.stages[event.stage] = {
      ...previous,
      status: event.status,
      ...(event.status === "running"
        ? { command: event.text }
        : {
            code: event.code,
            durationMs: event.durationMs,
            message: event.text,
          }),
    };
  }
  return next;
}
export function runText(
  run: Run | undefined,
  stage: Stage | "all",
  stream = "all",
  query = "",
  severity = "all",
  normalize = false,
) {
  if (!run) return "";
  return run.events
    .filter(
      (e) =>
        e.kind === "output" &&
        (stage === "all" || e.stage === stage) &&
        (stream === "all" || e.stream === stream),
    )
    .map((e) => ({ event: e, parsed: diagnostic(e.text) }))
    .filter(
      ({ parsed }) =>
        (severity === "all" || parsed.severity === severity) &&
        parsed.message.toLowerCase().includes(query.toLowerCase()),
    )
    .map(({ event }) => {
      let text = clean(event.text);
      if (normalize) {
        if (run.directory) text = text.replaceAll(run.directory, "<run>");
        text = text.replace(/\b\d+(?:\.\d+)?(?:ms|µs|ns|s)\b/g, "<duration>");
      }
      return text;
    })
    .join("");
}
export function filename(path: string) {
  return path.split(/[\\/]/).pop() || path;
}
export function relativePath(workspace: string, path: string) {
  const prefix = workspace.replace(/[\\/]$/, "");
  return path.startsWith(prefix + "/") || path.startsWith(prefix + "\\")
    ? path.slice(prefix.length + 1)
    : path;
}
