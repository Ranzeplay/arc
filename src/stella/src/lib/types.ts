export type Stage = "compile" | "decode" | "execute";
export type Status =
  | "idle"
  | "running"
  | "passed"
  | "failed"
  | "cancelled"
  | "skipped";
export type Action = Stage | "pipeline";
export interface Tool {
  mode: "project" | "binary";
  path: string;
  target: string;
  configuration: string;
  buildArgs: string[];
  extraArgs: string[];
}
export interface Profile {
  name: string;
  source: string;
  compiler: Tool;
  runtime: Tool;
  format: "legacy" | "ginkgo";
  logLevel: string;
  packageType: string;
  noStd: boolean;
  compress: boolean;
  verbose: boolean;
  decodeFlags: string[];
  args: string[];
  repeat: number;
  environment: Record<string, string>;
}
export interface Candidate {
  path: string;
  mode: "project" | "binary";
  label: string;
  target: string;
}
export interface Defaults {
  workspace: string;
  source: string;
  compiler: string;
  runtime: string;
}
export interface RunEvent {
  kind: "stage" | "output" | "finished";
  stage: Stage | "all";
  text: string;
  stream: "stdout" | "stderr" | "";
  status: Status;
  code: number | null;
  durationMs: number;
  time: number;
}
export interface StageResult {
  status: Status;
  command?: string;
  code?: number | null;
  durationMs?: number;
  message?: string;
}
export interface Run {
  id: string;
  name: string;
  started: number;
  status: Status;
  stages: Record<Stage, StageResult>;
  events: RunEvent[];
  directory: string;
  truncated?: boolean;
  profile: Profile;
}
export interface Diagnostic {
  severity: "error" | "warning" | "info" | "debug";
  message: string;
  path?: string;
  line?: number;
  column?: number;
}
export const stages: Stage[] = ["compile", "decode", "execute"];
export const stageNames: Record<Stage, string> = {
  compile: "Compilation",
  decode: "Decode",
  execute: "Execution",
};
export function defaultProfile(defaults: Defaults, name = "Baseline"): Profile {
  return {
    name,
    source: defaults.source,
    compiler: {
      mode: "project",
      path: defaults.compiler,
      target: "",
      configuration: "Debug",
      buildArgs: [],
      extraArgs: [],
    },
    runtime: {
      mode: "project",
      path: defaults.runtime,
      target: "arc_cli",
      configuration: "dev",
      buildArgs: [],
      extraArgs: [],
    },
    format: "legacy",
    logLevel: "Information",
    packageType: "Executable",
    noStd: false,
    compress: false,
    verbose: true,
    decodeFlags: ["descriptor", "symbols", "constants", "instructions"],
    args: [],
    repeat: 1,
    environment: {},
  };
}
