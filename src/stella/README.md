# Stella

A local Arc compiler and runtime workbench built with Tauri 2, React, shadcn/ui, and Shiki.

## Run

Install Node.js, pnpm, Rust, and the [Tauri platform prerequisites](https://v2.tauri.app/start/prerequisites/). Running the Arc compiler from its project also requires the .NET SDK specified in `src/compiler/global.json`.

```sh
cd src/stella
pnpm install
pnpm desktop
```

`pnpm dev` opens a frontend preview. Native file access, discovery, and processes run through the desktop app.

## Workflow

Stella detects the Arc repository from its launch directory or executable location. The initial source is `examples/helloWorld.script.arc`. Compiler and runtime paths default to the Circle project and runtime Cargo workspace. Paths can be typed, browsed, or reset. Saved paths within the workspace stay relative.

Choose a source file or directory. A directory includes its `.script.arc` files recursively. Choose **Project** to use `dotnet run` or `cargo run`, rebuilding current code as needed. Choose **Binary** to run an existing executable or a compiler DLL through `dotnet`.

The **Directory** and **Find** actions search .NET projects, `bin/<configuration>/<framework>` and publish output, Cargo projects, and `target/<profile>` or `target/<triple>/<profile>` binaries. Choose a candidate when several are available. Use the configuration/profile and Cargo binary fields to control project launches. Standard toolchain install locations supplement PATH for desktop launches.

**Run pipeline** compiles, decodes, and executes. The stage menu also provides compile-only, compile/decode, and compile/execute actions. A failed stage stops dependent work. Ginkgo execution is attempted by the runtime and may fail because Ginkgo execution support is not yet implemented.

Each run stores its request, full event log, package, and optional source-info in an isolated directory under the application's cache. The footer shows that directory. The UI retains the latest 20 runs for the session and the latest 10,000 events per run. Delete a selected run or clear all run sessions and cached artifacts from the history bar. Export selected results as JSON using the header download button. Profiles and paths persist locally between launches.

**Compare** opens independent A/B configurations. **Copy A to B** provides a starting point. **Run both** runs A followed by B to avoid competing builds. Select any recent run on either side, compare stage status/timing/exit codes, and inspect syntax-highlighted output with added and removed lines marked. Optional normalization replaces per-run artifact directories and durations for comparison.

**Inspect decode** pairs the selected Arc source with its `cmdec` output and shows the package, command, duration, and exit code. Drag the divider to resize either side, swap the panels, or stack them vertically. The layout persists between launches. Drag between stage cards to resize their widths, or use each card's corner handle to adjust its height.

Filter output by stage, severity, stream, or text. Compiler diagnostics link to the source preview. **Advanced options** accepts one argument per line, preserving spaces without shell interpretation. Environment overrides use one `KEY=value` per line. Runtime processes receive a closed stdin.

## Structure

- `src/App.tsx` owns profiles, run history, and desktop events.
- `src/components/` contains setup, source/output views, and local shadcn/ui components.
- `src/lib/` contains shared types, diagnostic parsing, and Arc syntax highlighting.
- `src-tauri/src/discovery.rs` resolves sources, projects, and binaries.
- `src-tauri/src/runner.rs` builds commands, streams output, manages artifacts, and cancels process trees.

## Verify and package

```sh
pnpm build
pnpm test
cargo test --manifest-path src-tauri/Cargo.toml
pnpm tauri build
```
