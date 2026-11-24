import {
  useCallback,
  useEffect,
  useLayoutEffect,
  useRef,
  useState,
} from "react";
import { Channel, invoke, isTauri } from "@tauri-apps/api/core";
import { getCurrentWindow } from "@tauri-apps/api/window";
import { confirm, save } from "@tauri-apps/plugin-dialog";
import {
  Braces,
  Check,
  ChevronRight,
  FileCode2,
  Download,
  GitCompareArrows,
  Loader2,
  Moon,
  PanelLeftClose,
  PanelLeftOpen,
  Play,
  RotateCcw,
  Square,
  Sun,
  Trash2,
  X,
} from "lucide-react";
import Setup, { Select } from "./components/Setup";
import Output, { StatusIcon } from "./components/Output";
import SourcePreview from "./components/SourcePreview";
import Inspector from "./components/Inspector";
import { Button } from "./components/ui/button";
import { Tabs, TabsList, TabsTrigger } from "./components/ui/tabs";
import {
  defaultProfile,
  type Action,
  type Defaults,
  type Diagnostic,
  type Profile,
  type Run,
  type RunEvent,
  type Status,
} from "./lib/types";
import { filename, reduceEvent } from "./lib/output";

const fallback: Defaults = {
  workspace: "",
  source: "examples/helloWorld.script.arc",
  compiler: "src/compiler/Executables/Circle/Arc.Compiler.Circle.csproj",
  runtime: "src/runtime/Cargo.toml",
};
const storageKey = "arc-stella-v1";
function readSaved(): { workspace?: string; profiles?: [Profile, Profile] } {
  try {
    const value = JSON.parse(localStorage.getItem(storageKey) || "null");
    if (
      value?.profiles?.length === 2 &&
      value.profiles.every(
        (p: Profile) =>
          typeof p.name === "string" &&
          p.compiler &&
          p.runtime &&
          Array.isArray(p.decodeFlags),
      )
    )
      return value;
  } catch {
    /* A damaged saved profile falls back to discovered defaults. */
  }
  return {};
}

export default function App() {
  const [theme, setTheme] = useState<"light" | "dark">(() => {
    try {
      const saved = localStorage.getItem("arc-stella-theme");
      if (saved === "light" || saved === "dark") return saved;
    } catch {
      /* Use the system preference when local storage is unavailable. */
    }
    return window.matchMedia("(prefers-color-scheme: dark)").matches
      ? "dark"
      : "light";
  });
  const [defaults, setDefaults] = useState(fallback);
  const [workspace, setWorkspace] = useState("");
  const [profiles, setProfiles] = useState<[Profile, Profile]>([
    defaultProfile(fallback),
    defaultProfile(fallback, "Candidate"),
  ]);
  const [active, setActive] = useState<0 | 1>(0);
  const [compare, setCompare] = useState(false);
  const [inspector, setInspector] = useState(false);
  const [sidebar, setSidebar] = useState(true);
  const [ready, setReady] = useState(false);
  const [error, setError] = useState("");
  const [sourceError, setSourceError] = useState("");
  const [sources, setSources] = useState<string[]>([]);
  const [path, setPath] = useState("");
  const [source, setSource] = useState("");
  const [sourceLoading, setSourceLoading] = useState(false);
  const [line, setLine] = useState<number>();
  const [runs, setRuns] = useState<Run[]>([]);
  const [selection, setSelection] = useState<[string, string]>(["", ""]);
  const [starting, setStarting] = useState(false);
  const lock = useRef(false);
  const channels = useRef(new Map<string, Channel<RunEvent>>());
  const profile = profiles[active];
  const running = runs.some((run) => run.status === "running") || starting;
  const selected = selection.map((id) => runs.find((run) => run.id === id)) as [
    Run | undefined,
    Run | undefined,
  ];
  const onError = useCallback((message: string) => setError(message), []);
  const editorTheme = `stella-${theme}`;

  useLayoutEffect(() => {
    document.documentElement.dataset.theme = theme;
    document
      .querySelector('meta[name="theme-color"]')
      ?.setAttribute("content", theme === "dark" ? "#111216" : "#f8f9fc");
    try {
      localStorage.setItem("arc-stella-theme", theme);
    } catch {
      setError("Unable to save the selected theme in local app storage.");
    }
  }, [theme]);

  useEffect(() => {
    if (isTauri()) {
      getCurrentWindow()
        .setTheme(theme)
        .catch((error) => setError(String(error)));
    }
  }, [theme]);

  useEffect(() => {
    let disposed = false;
    async function initialize() {
      try {
        const discovered = isTauri()
          ? await invoke<Defaults>("bootstrap")
          : fallback;
        if (disposed) return;
        const saved = readSaved();
        setDefaults(discovered);
        setWorkspace(saved.workspace || discovered.workspace);
        setProfiles(
          saved.profiles || [
            defaultProfile(discovered),
            defaultProfile(discovered, "Candidate"),
          ],
        );
        setReady(true);
      } catch (error) {
        if (!disposed) {
          setError(String(error));
          setReady(true);
        }
      }
    }
    initialize();
    return () => {
      disposed = true;
    };
  }, []);
  useEffect(() => {
    if (!ready) return;
    try {
      localStorage.setItem(storageKey, JSON.stringify({ workspace, profiles }));
    } catch {
      setError("Unable to save profiles in local app storage.");
    }
  }, [workspace, profiles, ready]);
  useEffect(() => {
    let disposed = false;
    const timer = setTimeout(async () => {
      if (!ready || !workspace || !profile.source || !isTauri()) {
        setSources([]);
        return;
      }
      try {
        const files = await invoke<string[]>("source_files", {
          workspace,
          path: profile.source,
        });
        if (!disposed) {
          setSources(files);
          setPath((current) => (files.includes(current) ? current : files[0]));
          setSourceError("");
        }
      } catch (error) {
        if (!disposed) {
          setSources([]);
          setPath("");
          setSourceError(String(error));
        }
      }
    }, 300);
    return () => {
      disposed = true;
      clearTimeout(timer);
    };
  }, [workspace, profile.source, ready]);
  useEffect(() => {
    let disposed = false;
    if (!path || !isTauri()) {
      setSource("");
      return;
    }
    setSourceLoading(true);
    invoke<string>("read_source", { workspace, path })
      .then((text) => {
        if (!disposed) setSource(text);
      })
      .catch((error) => {
        if (!disposed) setError(String(error));
      })
      .finally(() => {
        if (!disposed) setSourceLoading(false);
      });
    return () => {
      disposed = true;
    };
  }, [path, workspace, selected[0]?.id, selected[1]?.id]);

  function updateProfile(next: Profile) {
    setProfiles((current) =>
      active === 0 ? [next, current[1]] : [current[0], next],
    );
  }
  async function launch(
    index: 0 | 1,
    action: Action,
    waitForFinish: boolean,
  ): Promise<Status | undefined> {
    const snapshot = structuredClone(profiles[index]);
    const files = await invoke<string[]>("source_files", {
      workspace,
      path: snapshot.source,
    });
    const id = crypto.randomUUID();
    const run: Run = {
      id,
      name: snapshot.name,
      profile: snapshot,
      started: Date.now(),
      status: "running",
      directory: "",
      events: [],
      stages: {
        compile: { status: "idle" },
        decode: { status: "idle" },
        execute: { status: "idle" },
      },
    };
    setRuns((current) => [run, ...current].slice(0, 20));
    setSelection((current) =>
      index === 0 ? [id, current[1]] : [current[0], id],
    );
    let finish: (status: Status) => void = () => {};
    const completed = new Promise<Status>((resolve) => {
      finish = resolve;
    });
    const channel = new Channel<RunEvent>();
    channel.onmessage = (event) => {
      setRuns((current) =>
        current.map((item) =>
          item.id === id ? reduceEvent(item, event) : item,
        ),
      );
      if (event.kind === "finished") {
        channels.current.delete(id);
        finish(event.status);
      }
    };
    channels.current.set(id, channel);
    try {
      const directory = await invoke<string>("start_run", {
        request: { ...snapshot, id, workspace, sources: files, action },
        onEvent: channel,
      });
      setRuns((current) =>
        current.map((item) => (item.id === id ? { ...item, directory } : item)),
      );
      if (waitForFinish) return await completed;
    } catch (error) {
      channels.current.delete(id);
      const event: RunEvent = {
        kind: "finished",
        stage: "all",
        text: String(error),
        status: "failed",
        stream: "",
        code: null,
        durationMs: 0,
        time: Date.now(),
      };
      setRuns((current) =>
        current.map((item) =>
          item.id === id ? reduceEvent(item, event) : item,
        ),
      );
      throw error;
    }
  }
  async function run(action: Action, both = false) {
    if (lock.current) return;
    if (!isTauri()) {
      setError(
        "Open Stella with pnpm desktop to access local files and run compiler/runtime processes.",
      );
      return;
    }
    lock.current = true;
    setStarting(true);
    setError("");
    try {
      const status = await launch(both ? 0 : active, action, both);
      if (both && status !== "cancelled") await launch(1, action, true);
    } catch (error) {
      setError(String(error));
    } finally {
      setStarting(false);
      lock.current = false;
    }
  }
  async function stop() {
    const results = await Promise.allSettled(
      runs
        .filter((run) => run.status === "running")
        .map((run) => invoke("stop_run", { id: run.id })),
    );
    const failure = results.find((result) => result.status === "rejected");
    if (failure?.status === "rejected") setError(String(failure.reason));
  }
  async function deleteRun(id: string) {
    if (running) return;
    const run = runs.find((item) => item.id === id);
    if (!run) return;
    try {
      const approved = await confirm(
        `Delete ${run.name} and its cached build, decode, and execution output?`,
        { title: "Delete run session", kind: "warning" },
      );
      if (!approved) return;
      await invoke("delete_run", { id });
      setRuns((current) => current.filter((item) => item.id !== id));
      setSelection(
        (current) =>
          current.map((selectedId) =>
            selectedId === id ? "" : selectedId,
          ) as [string, string],
      );
    } catch (error) {
      setError(String(error));
    }
  }
  async function clearRuns() {
    if (running || !runs.length) return;
    try {
      const approved = await confirm(
        `Delete all ${runs.length} run sessions and cached artifacts?`,
        { title: "Clear all sessions", kind: "warning" },
      );
      if (!approved) return;
      await invoke("clear_runs");
      setRuns([]);
      setSelection(["", ""]);
    } catch (error) {
      setError(String(error));
    }
  }
  async function exportRuns() {
    try {
      const path = await save({
        title: "Export run results",
        defaultPath: "stella-results.json",
        filters: [{ name: "JSON", extensions: ["json"] }],
      });
      if (path)
        await invoke("export_run", {
          path,
          content: JSON.stringify(
            {
              workspace,
              runs: compare
                ? selected.filter(Boolean)
                : selected.slice(0, 1).filter(Boolean),
            },
            null,
            2,
          ),
        });
    } catch (error) {
      setError(String(error));
    }
  }
  function navigate(diagnostic: Diagnostic) {
    if (diagnostic.path) {
      setPath(diagnostic.path);
      setLine(diagnostic.line);
    }
  }
  return (
    <div className="app-shell">
      <header className="app-header">
        <div className="brand">
          <div className="brand-mark">
            <Braces size={23} strokeWidth={1.8} />
          </div>
          <span>
            stella<span className="brand-dot">.</span>
          </span>
          <span className="brand-divider" />
          <span className="brand-description">Arc workbench</span>
        </div>
        <div className="header-actions">
          <span className={`connection ${isTauri() ? "native" : ""}`}>
            <span />
            {isTauri() ? "Local environment" : "Browser preview"}
          </span>
          <Button
            variant="ghost"
            size="icon"
            title={`Switch to ${theme === "dark" ? "light" : "dark"} mode`}
            aria-label={`Switch to ${theme === "dark" ? "light" : "dark"} mode`}
            onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
          >
            {theme === "dark" ? <Sun /> : <Moon />}
          </Button>
          <Button
            variant={compare ? "secondary" : "outline"}
            size="sm"
            onClick={() => {
              setCompare(!compare);
              if (compare) setActive(0);
            }}
          >
            <GitCompareArrows />
            Compare
          </Button>
          <Button
            variant="ghost"
            size="icon"
            title="Export selected results"
            aria-label="Export selected results"
            onClick={exportRuns}
            disabled={!selected.some(Boolean)}
          >
            <Download />
          </Button>
        </div>
      </header>
      <div className="workspace-bar">
        <Button
          variant="ghost"
          size="icon"
          onClick={() => setSidebar(!sidebar)}
          title="Toggle configuration"
          aria-label="Toggle configuration"
        >
          {sidebar ? <PanelLeftClose /> : <PanelLeftOpen />}
        </Button>
        <span className="workspace-name">
          {filename(workspace) || "Select workspace"}
        </span>
        <ChevronRight size={13} />
        <span>{filename(profile.source)}</span>
        <span className="workspace-spacer" />
        <span className="format-label">
          {profile.format === "ginkgo" ? "Ginkgo" : "Legacy"}
        </span>
        <span className="source-count">
          {sources.length} source {sources.length === 1 ? "file" : "files"}
        </span>
      </div>
      {error && (
        <div className="error-banner" role="alert">
          <span>{error}</span>
          <Button
            variant="ghost"
            size="icon"
            aria-label="Dismiss error"
            onClick={() => setError("")}
          >
            <X />
          </Button>
        </div>
      )}
      <div className="app-body">
        {sidebar && (
          <aside className="sidebar">
            <div className="sidebar-heading">
              <div>
                <span className="eyebrow">CONFIGURATION</span>
                <h2>Run setup</h2>
              </div>
              <Button
                variant="ghost"
                size="icon"
                title="Restore profile defaults"
                aria-label="Restore profile defaults"
                disabled={running}
                onClick={() =>
                  updateProfile(
                    defaultProfile(
                      defaults,
                      active === 0 ? "Baseline" : "Candidate",
                    ),
                  )
                }
              >
                <RotateCcw />
              </Button>
            </div>
            {compare && (
              <div className="profile-switch">
                <Tabs
                  value={String(active)}
                  onValueChange={(value) => setActive(+value as 0 | 1)}
                >
                  <TabsList className="w-full">
                    <TabsTrigger className="flex-1" value="0">
                      A · {profiles[0].name}
                    </TabsTrigger>
                    <TabsTrigger className="flex-1" value="1">
                      B · {profiles[1].name}
                    </TabsTrigger>
                  </TabsList>
                </Tabs>
                <Button
                  variant="ghost"
                  size="sm"
                  disabled={running}
                  onClick={() =>
                    setProfiles((current) => [
                      current[0],
                      { ...structuredClone(current[0]), name: current[1].name },
                    ])
                  }
                >
                  Copy A to B
                </Button>
              </div>
            )}
            <Setup
              key={active}
              workspace={workspace}
              setWorkspace={setWorkspace}
              profile={profile}
              onChange={updateProfile}
              defaults={defaults}
              onError={onError}
              disabled={running || !ready}
            />
          </aside>
        )}
        <main className="main-workspace">
          <div className="main-toolbar">
            <div className="run-actions">
              <Button
                variant={inspector ? "secondary" : "outline"}
                onClick={() => setInspector(!inspector)}
                aria-pressed={inspector}
              >
                <FileCode2 />
                {inspector ? "Pipeline view" : "Inspect decode"}
              </Button>
              <Button
                onClick={() => run("pipeline")}
                disabled={running || !ready}
              >
                {starting ? (
                  <Loader2 className="animate-spin" />
                ) : (
                  <Play fill="currentColor" />
                )}
                Run {compare ? (active === 0 ? "A" : "B") : "pipeline"}
              </Button>
              {compare && (
                <Button
                  variant="outline"
                  onClick={() => run("pipeline", true)}
                  disabled={running}
                >
                  Run both
                </Button>
              )}
              <Button variant="outline" onClick={stop} disabled={!running}>
                <Square size={13} />
                Stop
              </Button>
              <div className="toolbar-divider" />
              <Select
                aria-label="Run individual stage"
                value=""
                disabled={running}
                onChange={(e) => run(e.target.value as Action)}
              >
                <option value="" disabled>
                  Run stage…
                </option>
                <option value="compile">Compile only</option>
                <option value="decode">Compile + decode</option>
                <option value="execute">Compile + execute</option>
              </Select>
            </div>
            <span className="run-state">
              {running ? (
                <>
                  <Loader2 size={13} className="animate-spin" />
                  Running
                </>
              ) : (
                <>
                  <Check size={13} />
                  {selected[active] ? "Finished" : "Ready to run"}
                </>
              )}
            </span>
          </div>
          <div className="workbench-content">
            {inspector ? (
              <Inspector
                editorTheme={editorTheme}
                sources={sources}
                path={path}
                setPath={(value) => {
                  setPath(value);
                  setLine(undefined);
                }}
                source={source}
                loading={sourceLoading}
                runs={selected}
                compare={compare}
                active={active}
              />
            ) : (
              <SourcePreview
                editorTheme={editorTheme}
                sources={sources}
                path={path}
                setPath={(value) => {
                  setPath(value);
                  setLine(undefined);
                }}
                text={source}
                loading={sourceLoading}
                line={line}
              />
            )}
            {sourceError && <p className="source-error">{sourceError}</p>}
            <div className="history-bar">
              {(compare ? [0, 1] : [0]).map((index) => (
                <div className="history-select" key={index}>
                  <span className={`lane-tag lane-${index}`}>
                    {index === 0 ? "A" : "B"}
                  </span>
                  <Select
                    aria-label={`Run ${index === 0 ? "A" : "B"} history`}
                    value={selection[index]}
                    onChange={(e) =>
                      setSelection((current) =>
                        index === 0
                          ? [e.target.value, current[1]]
                          : [current[0], e.target.value],
                      )
                    }
                  >
                    <option value="">
                      {index === 0 ? profiles[0].name : profiles[1].name} · no
                      run selected
                    </option>
                    {runs.map((run) => (
                      <option value={run.id} key={run.id}>
                        {run.name} ·{" "}
                        {new Date(run.started).toLocaleTimeString()} ·{" "}
                        {run.status}
                      </option>
                    ))}
                  </Select>
                  <Button
                    variant="ghost"
                    size="icon"
                    title={`Delete selected ${index === 0 ? "A" : "B"} run session`}
                    aria-label={`Delete selected ${index === 0 ? "A" : "B"} run session`}
                    disabled={running || !selection[index]}
                    onClick={() => deleteRun(selection[index])}
                  >
                    <Trash2 />
                  </Button>
                </div>
              ))}
              <div className="history-actions">
                <span className="history-hint">
                  Recent runs stay available during this session.
                </span>
                <Button
                  variant="ghost"
                  size="sm"
                  disabled={running || runs.length === 0}
                  onClick={clearRuns}
                >
                  <Trash2 />
                  Clear all sessions
                </Button>
              </div>
            </div>
            <Output
              editorTheme={editorTheme}
              runs={selected}
              compare={compare}
              onLocation={navigate}
              onError={onError}
            />
          </div>
        </main>
      </div>
      <footer className="app-footer">
        <span>
          <span className={`status-dot ${running ? "busy" : ""}`} />
          {running ? "Processes active" : "Stella"}
        </span>
        <span>
          {profile.compiler.mode === "project"
            ? "dotnet run"
            : "Compiler binary"}
          <span className="footer-separator">/</span>
          {profile.runtime.mode === "project" ? "cargo run" : "Runtime binary"}
        </span>
        <span className="footer-right">
          {selected[active]?.directory || "Isolated artifacts for every run"}
        </span>
        {selected[active] && <StatusIcon status={selected[active]!.status} />}
      </footer>
    </div>
  );
}
