import { useState } from "react";
import { open } from "@tauri-apps/plugin-dialog";
import { invoke } from "@tauri-apps/api/core";
import {
  FileCode2,
  FolderOpen,
  RotateCcw,
  Search,
  Cpu,
  Box,
  ChevronDown,
  Loader2,
} from "lucide-react";
import { Button } from "./ui/button";
import { Input } from "./ui/input";
import { Tabs, TabsList, TabsTrigger } from "./ui/tabs";
import type { Candidate, Defaults, Profile, Tool } from "@/lib/types";
import { relativePath } from "@/lib/output";

export function Select({ children, ...props }: React.ComponentProps<"select">) {
  return (
    <select {...props} className={`select ${props.className || ""}`}>
      {children}
    </select>
  );
}
export function Check({
  label,
  ...props
}: React.ComponentProps<"input"> & { label: string }) {
  return (
    <label className="check">
      <input type="checkbox" {...props} />
      <span>{label}</span>
    </label>
  );
}
export function Field({
  label,
  children,
  hint,
}: {
  label: string;
  children: React.ReactNode;
  hint?: string;
}) {
  return (
    <label className="field">
      <span>{label}</span>
      {children}
      {hint && <small>{hint}</small>}
    </label>
  );
}

function ToolPicker({
  kind,
  tool,
  workspace,
  reset,
  onChange,
  onError,
}: {
  kind: "compiler" | "runtime";
  tool: Tool;
  workspace: string;
  reset: string;
  onChange: (tool: Tool) => void;
  onError: (message: string) => void;
}) {
  const [candidates, setCandidates] = useState<Candidate[]>([]);
  const [searching, setSearching] = useState(false);
  const [searched, setSearched] = useState(false);
  const patch = (value: Partial<Tool>) => onChange({ ...tool, ...value });
  async function discover(path = tool.path) {
    setSearching(true);
    setSearched(false);
    try {
      const found = await invoke<Candidate[]>("discover", {
        workspace,
        path,
        kind,
      });
      setCandidates(found);
      setSearched(true);
      if (found.length === 1) pick(found[0]);
      if (!found.length)
        onError(
          `No runnable ${kind} projects or binaries found. Select a project file, build output, or a narrower directory.`,
        );
    } catch (error) {
      onError(String(error));
    } finally {
      setSearching(false);
    }
  }
  function pick(candidate: Candidate) {
    onChange({
      ...tool,
      mode: candidate.mode,
      path: relativePath(workspace, candidate.path),
      target: candidate.target,
    });
  }
  async function browse(directory: boolean) {
    try {
      const path = await open({
        directory,
        multiple: false,
        title: `Locate ${kind} ${directory ? "directory" : "project or binary"}`,
      });
      if (typeof path === "string") {
        patch({ path: relativePath(workspace, path) });
        await discover(path);
      }
    } catch (error) {
      onError(String(error));
    }
  }
  return (
    <section className="setup-section">
      <div className="section-heading">
        {kind === "compiler" ? <Cpu size={15} /> : <Box size={15} />}
        <h3>{kind === "compiler" ? "Compiler" : "Runtime"}</h3>
        <Button
          variant="ghost"
          size="icon"
          title={`Reset ${kind} path`}
          aria-label={`Reset ${kind} path`}
          onClick={() => {
            patch({
              path: reset,
              mode: "project",
              target: kind === "runtime" ? "arc_cli" : "",
            });
            setCandidates([]);
            setSearched(false);
          }}
        >
          <RotateCcw />
        </Button>
      </div>
      <Tabs
        value={tool.mode}
        onValueChange={(mode) => patch({ mode: mode as Tool["mode"] })}
      >
        <TabsList className="w-full">
          <TabsTrigger className="flex-1" value="project">
            Project
          </TabsTrigger>
          <TabsTrigger className="flex-1" value="binary">
            Binary
          </TabsTrigger>
        </TabsList>
      </Tabs>
      <Field
        label={
          tool.mode === "project"
            ? "Project or directory"
            : "Binary or directory"
        }
      >
        <Input
          value={tool.path}
          onChange={(e) => {
            patch({ path: e.target.value });
            setSearched(false);
          }}
          title={tool.path}
          placeholder={
            kind === "compiler"
              ? "Path to .csproj or compiler"
              : "Path to Cargo.toml or runtime"
          }
        />
      </Field>
      <div className="path-actions">
        <Button variant="outline" size="sm" onClick={() => browse(true)}>
          <FolderOpen />
          Directory
        </Button>
        <Button variant="outline" size="sm" onClick={() => browse(false)}>
          <FileCode2 />
          File
        </Button>
        <Button
          variant="ghost"
          size="sm"
          disabled={searching || !tool.path}
          onClick={() => discover()}
        >
          {searching ? <Loader2 className="animate-spin" /> : <Search />}Find
        </Button>
      </div>
      {searched && candidates.length > 0 && (
        <Field
          label={`${candidates.length} ${candidates.length === 1 ? "match" : "matches"} found`}
        >
          <Select
            aria-label={`${kind} discovery results`}
            value={candidates.findIndex(
              (c) =>
                relativePath(workspace, c.path) === tool.path &&
                c.target === tool.target,
            )}
            onChange={(e) => pick(candidates[+e.target.value])}
          >
            <option value={-1} disabled>
              Choose a project or binary
            </option>
            {candidates.map((c, index) => (
              <option key={`${c.path}:${c.target}`} value={index}>
                {c.label} — {relativePath(workspace, c.path)}
              </option>
            ))}
          </Select>
        </Field>
      )}
      {tool.mode === "project" && (
        <div className="two-fields">
          <Field label={kind === "compiler" ? "Configuration" : "Profile"}>
            <Input
              aria-label={`${kind} build profile`}
              value={tool.configuration}
              onChange={(e) => patch({ configuration: e.target.value })}
              list={`${kind}-profiles`}
            />
            <datalist id={`${kind}-profiles`}>
              {(kind === "compiler"
                ? ["Debug", "Release"]
                : ["dev", "release", "profiling"]
              ).map((p) => (
                <option key={p}>{p}</option>
              ))}
            </datalist>
          </Field>
          {kind === "runtime" && (
            <Field label="Cargo binary">
              <Input
                value={tool.target}
                onChange={(e) => patch({ target: e.target.value })}
                placeholder="arc_cli"
              />
            </Field>
          )}
        </div>
      )}
      <p className="field-note">
        {tool.mode === "project"
          ? `${kind === "compiler" ? "dotnet run" : "cargo run"} builds current code before each stage.`
          : "Runs the selected build directly."}
      </p>
    </section>
  );
}

export default function Setup({
  workspace,
  setWorkspace,
  profile,
  onChange,
  defaults,
  onError,
  disabled,
}: {
  workspace: string;
  setWorkspace: (value: string) => void;
  profile: Profile;
  onChange: (profile: Profile) => void;
  defaults: Defaults;
  onError: (message: string) => void;
  disabled: boolean;
}) {
  const patch = (value: Partial<Profile>) => onChange({ ...profile, ...value });
  async function browseSource(directory: boolean) {
    try {
      const path = await open({
        directory,
        multiple: false,
        title: directory ? "Select source directory" : "Select Arc source",
        ...(!directory && {
          filters: [{ name: "Arc source", extensions: ["arc"] }],
        }),
      });
      if (typeof path === "string")
        patch({ source: relativePath(workspace, path) });
    } catch (error) {
      onError(String(error));
    }
  }
  return (
    <fieldset className="setup-fields" disabled={disabled}>
      <section className="setup-section workspace-section">
        <Field label="Workspace">
          <div className="input-action">
            <Input
              aria-label="Workspace path"
              value={workspace}
              onChange={(e) => setWorkspace(e.target.value)}
              title={workspace}
            />
            <Button
              variant="ghost"
              size="icon"
              aria-label="Browse workspace"
              title="Browse workspace"
              onClick={async () => {
                try {
                  const path = await open({
                    directory: true,
                    title: "Select Arc workspace",
                  });
                  if (typeof path === "string") setWorkspace(path);
                } catch (e) {
                  onError(String(e));
                }
              }}
            >
              <FolderOpen />
            </Button>
          </div>
        </Field>
        <Field label="Profile name">
          <Input
            value={profile.name}
            onChange={(e) => patch({ name: e.target.value })}
          />
        </Field>
      </section>
      <section className="setup-section">
        <div className="section-heading">
          <FileCode2 size={15} />
          <h3>Source</h3>
          <Button
            variant="ghost"
            size="icon"
            title="Reset source path"
            aria-label="Reset source path"
            onClick={() => patch({ source: defaults.source })}
          >
            <RotateCcw />
          </Button>
        </div>
        <Field label="File or directory">
          <Input
            aria-label="Source path"
            value={profile.source}
            onChange={(e) => patch({ source: e.target.value })}
            title={profile.source}
          />
        </Field>
        <div className="path-actions">
          <Button
            variant="outline"
            size="sm"
            onClick={() => browseSource(true)}
          >
            <FolderOpen />
            Directory
          </Button>
          <Button
            variant="outline"
            size="sm"
            onClick={() => browseSource(false)}
          >
            <FileCode2 />
            File
          </Button>
        </div>
        <p className="field-note">
          Directories include all .script.arc files recursively.
        </p>
      </section>
      <ToolPicker
        kind="compiler"
        tool={profile.compiler}
        workspace={workspace}
        reset={defaults.compiler}
        onChange={(compiler) => patch({ compiler })}
        onError={onError}
      />
      <ToolPicker
        kind="runtime"
        tool={profile.runtime}
        workspace={workspace}
        reset={defaults.runtime}
        onChange={(runtime) => patch({ runtime })}
        onError={onError}
      />
      <section className="setup-section">
        <div className="section-heading">
          <h3>Pipeline options</h3>
        </div>
        <div className="two-fields">
          <Field label="Package format">
            <Select
              value={profile.format}
              onChange={(e) =>
                patch({ format: e.target.value as Profile["format"] })
              }
            >
              <option value="legacy">Legacy</option>
              <option value="ginkgo">Ginkgo</option>
            </Select>
          </Field>
          <Field label="Log level">
            <Select
              value={profile.logLevel}
              onChange={(e) => patch({ logLevel: e.target.value })}
            >
              {[
                "Trace",
                "Debug",
                "Information",
                "Warning",
                "Error",
                "Critical",
              ].map((level) => (
                <option key={level}>{level}</option>
              ))}
            </Select>
          </Field>
        </div>
        <Field label="Package type">
          <Select
            value={profile.packageType}
            onChange={(e) => patch({ packageType: e.target.value })}
          >
            <option>Executable</option>
            <option>Library</option>
          </Select>
        </Field>
        <Check
          label="Include standard library"
          checked={!profile.noStd}
          onChange={(e) => patch({ noStd: !e.target.checked })}
        />
        <Check
          label="Verbose runtime output"
          checked={profile.verbose}
          onChange={(e) => patch({ verbose: e.target.checked })}
        />
        {profile.format === "ginkgo" && (
          <>
            <Check
              label="Compress package (Zstandard)"
              checked={profile.compress}
              onChange={(e) => patch({ compress: e.target.checked })}
            />
            <p className="inline-note">
              Execution will be attempted. The current runtime may reject Ginkgo
              packages.
            </p>
          </>
        )}
        <span className="control-label">Decode output</span>
        <div className="checks-grid">
          {["descriptor", "symbols", "constants", "instructions", "raw"].map(
            (flag) => (
              <Check
                key={flag}
                label={flag.charAt(0).toUpperCase() + flag.slice(1)}
                checked={profile.decodeFlags.includes(flag)}
                onChange={(e) =>
                  patch({
                    decodeFlags: e.target.checked
                      ? [...profile.decodeFlags, flag]
                      : profile.decodeFlags.filter((f) => f !== flag),
                  })
                }
              />
            ),
          )}
        </div>
      </section>
      <details className="setup-section advanced">
        <summary>
          Advanced options
          <ChevronDown size={14} />
        </summary>
        <Field label="Execution repeat">
          <Input
            type="number"
            min={1}
            max={100000}
            value={profile.repeat}
            onChange={(e) => patch({ repeat: Math.max(1, +e.target.value) })}
          />
        </Field>
        <Field
          label="Program arguments"
          hint="One argument per line. Spaces are preserved."
        >
          <textarea
            value={profile.args.join("\n")}
            onChange={(e) =>
              patch({ args: e.target.value ? e.target.value.split("\n") : [] })
            }
            rows={3}
          />
        </Field>
        {(["compiler", "runtime"] as const).map((kind) => (
          <div key={kind} className="advanced-tool">
            <span className="control-label capitalize">{kind}</span>
            {(["buildArgs", "extraArgs"] as const).map((field) => (
              <Field
                key={field}
                label={
                  field === "buildArgs" ? "Build arguments" : "CLI arguments"
                }
                hint="One argument per line."
              >
                <textarea
                  rows={2}
                  value={profile[kind][field].join("\n")}
                  onChange={(e) =>
                    patch({
                      [kind]: {
                        ...profile[kind],
                        [field]: e.target.value
                          ? e.target.value.split("\n")
                          : [],
                      },
                    })
                  }
                />
              </Field>
            ))}
          </div>
        ))}
        <Field
          label="Environment overrides"
          hint="One KEY=value per line. Inherits the desktop environment."
        >
          <textarea
            rows={3}
            defaultValue={Object.entries(profile.environment)
              .map(([key, value]) => `${key}=${value}`)
              .join("\n")}
            key={profile.name}
            onBlur={(e) => {
              const environment: Record<string, string> = {};
              for (const line of e.target.value.split("\n").filter(Boolean)) {
                const at = line.indexOf("=");
                if (at <= 0) {
                  onError("Environment entries must use KEY=value.");
                  return;
                }
                environment[line.slice(0, at)] = line.slice(at + 1);
              }
              patch({ environment });
            }}
          />
        </Field>
      </details>
    </fieldset>
  );
}
