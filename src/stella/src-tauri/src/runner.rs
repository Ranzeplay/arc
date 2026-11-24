use crate::discovery::resolve;
use serde::{Deserialize, Serialize};
use std::{
    collections::HashMap,
    fs::File,
    io::Write,
    path::{Path, PathBuf},
    process::Stdio,
    sync::{
        atomic::{AtomicU32, Ordering},
        Arc, Mutex,
    },
    time::Instant,
};
use tauri::{ipc::Channel, Manager, State};
use tokio::{
    io::{AsyncRead, AsyncReadExt},
    process::Command,
    sync::watch,
};

#[derive(Clone, Deserialize, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct Tool {
    mode: String,
    path: String,
    target: String,
    configuration: String,
    build_args: Vec<String>,
    extra_args: Vec<String>,
}

#[derive(Clone, Deserialize, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct Request {
    id: String,
    workspace: String,
    sources: Vec<String>,
    compiler: Tool,
    runtime: Tool,
    format: String,
    log_level: String,
    package_type: String,
    no_std: bool,
    compress: bool,
    verbose: bool,
    decode_flags: Vec<String>,
    args: Vec<String>,
    repeat: u32,
    action: String,
    environment: HashMap<String, String>,
}

#[derive(Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct Event {
    kind: String,
    stage: String,
    text: String,
    stream: String,
    status: String,
    code: Option<i32>,
    duration_ms: u64,
    time: u128,
}

impl Event {
    fn new(kind: &str, stage: &str, text: impl Into<String>) -> Self {
        Self {
            kind: kind.into(),
            stage: stage.into(),
            text: text.into(),
            stream: String::new(),
            status: String::new(),
            code: None,
            duration_ms: 0,
            time: std::time::SystemTime::now()
                .duration_since(std::time::UNIX_EPOCH)
                .unwrap_or_default()
                .as_millis(),
        }
    }
}

#[derive(Clone)]
struct Reporter {
    channel: Channel<Event>,
    log: Arc<Mutex<File>>,
}
impl Reporter {
    fn send(&self, event: Event) {
        if let Ok(mut file) = self.log.lock() {
            if let Ok(json) = serde_json::to_string(&event) {
                let _ = writeln!(file, "{json}");
            }
        }
        let _ = self.channel.send(event);
    }
}

#[derive(Default)]
pub struct Runs(Mutex<HashMap<String, ActiveRun>>);
struct ActiveRun {
    cancel: watch::Sender<bool>,
    pid: Arc<AtomicU32>,
}
impl Runs {
    pub fn stop_all(&self) {
        if let Ok(runs) = self.0.lock() {
            for run in runs.values() {
                let _ = run.cancel.send(true);
                terminate_tree(Some(run.pid.swap(0, Ordering::SeqCst)));
            }
        }
    }
}

#[tauri::command]
pub fn stop_run(id: String, runs: State<'_, Runs>) -> Result<(), String> {
    let runs = runs.0.lock().map_err(|e| e.to_string())?;
    if let Some(run) = runs.get(&id) {
        let _ = run.cancel.send(true);
    }
    Ok(())
}

fn run_directory(runs_root: &Path, id: &str) -> Result<PathBuf, String> {
    if id.is_empty()
        || !id
            .chars()
            .all(|character| character.is_ascii_alphanumeric() || character == '-')
    {
        return Err("Invalid run ID".into());
    }
    Ok(runs_root.join(id))
}

#[tauri::command]
pub fn delete_run(id: String, app: tauri::AppHandle, runs: State<'_, Runs>) -> Result<(), String> {
    let active = runs.0.lock().map_err(|error| error.to_string())?;
    if active.contains_key(&id) {
        return Err("Stop the active run before deleting its session.".into());
    }
    let root = app
        .path()
        .app_cache_dir()
        .map_err(|error| error.to_string())?
        .join("runs");
    let directory = run_directory(&root, &id)?;
    if directory.exists() {
        std::fs::remove_dir_all(directory).map_err(|error| error.to_string())?;
    }
    Ok(())
}

#[tauri::command]
pub fn clear_runs(app: tauri::AppHandle, runs: State<'_, Runs>) -> Result<(), String> {
    let active = runs.0.lock().map_err(|error| error.to_string())?;
    if !active.is_empty() {
        return Err("Stop active runs before clearing session history.".into());
    }
    let directory = app
        .path()
        .app_cache_dir()
        .map_err(|error| error.to_string())?
        .join("runs");
    if directory.exists() {
        std::fs::remove_dir_all(&directory).map_err(|error| error.to_string())?;
    }
    std::fs::create_dir_all(directory).map_err(|error| error.to_string())?;
    Ok(())
}

#[tauri::command]
pub async fn start_run(
    app: tauri::AppHandle,
    runs: State<'_, Runs>,
    request: Request,
    on_event: Channel<Event>,
) -> Result<String, String> {
    if request.id.is_empty()
        || !request
            .id
            .chars()
            .all(|c| c.is_ascii_alphanumeric() || c == '-')
    {
        return Err("Invalid run ID".into());
    }
    if request.sources.is_empty() {
        return Err("Select at least one source file.".into());
    }
    if !matches!(
        request.action.as_str(),
        "compile" | "decode" | "execute" | "pipeline"
    ) {
        return Err("Unknown action".into());
    }
    let workspace = Path::new(&request.workspace);
    if !workspace.is_dir() {
        return Err("Workspace directory does not exist.".into());
    }
    for source in &request.sources {
        if !resolve(workspace, source).is_file() {
            return Err(format!("Source file does not exist: {source}"));
        }
    }
    let directory = app
        .path()
        .app_cache_dir()
        .map_err(|e| e.to_string())?
        .join("runs")
        .join(&request.id);
    std::fs::create_dir_all(directory.parent().unwrap()).map_err(|e| e.to_string())?;
    std::fs::create_dir(&directory).map_err(|e| e.to_string())?;
    std::fs::write(
        directory.join("request.json"),
        serde_json::to_vec_pretty(&request).map_err(|e| e.to_string())?,
    )
    .map_err(|e| e.to_string())?;
    let log = File::create(directory.join("events.jsonl")).map_err(|e| e.to_string())?;
    let (cancel, receiver) = watch::channel(false);
    let pid = Arc::new(AtomicU32::new(0));
    {
        let mut active = runs.0.lock().map_err(|e| e.to_string())?;
        if active.contains_key(&request.id) {
            return Err("This run is already active.".into());
        }
        active.insert(
            request.id.clone(),
            ActiveRun {
                cancel,
                pid: pid.clone(),
            },
        );
    }
    let result_path = directory.to_string_lossy().into_owned();
    tauri::async_runtime::spawn(async move {
        let reporter = Reporter {
            channel: on_event,
            log: Arc::new(Mutex::new(log)),
        };
        let outcome = pipeline(&request, &directory, &reporter, receiver, &pid).await;
        let mut event = Event::new("finished", "all", directory.to_string_lossy());
        match outcome {
            Ok(status) => event.status = status,
            Err(error) => {
                event.status = "failed".into();
                event.text = error;
            }
        }
        reporter.send(event);
        if let Ok(mut active) = app.state::<Runs>().0.lock() {
            active.remove(&request.id);
        }
    });
    Ok(result_path)
}

fn command_for(tool: &Tool, workspace: &Path) -> Result<(String, Vec<String>), String> {
    let path = resolve(workspace, &tool.path);
    if !path.is_file() {
        return Err(format!(
            "Tool file does not exist: {}. Use Discover to select a project or binary.",
            path.display()
        ));
    }
    let path = path.to_string_lossy().into_owned();
    if tool.mode == "binary" {
        if Path::new(&path).extension().is_some_and(|e| e == "dll") {
            return Ok(("dotnet".into(), vec![path]));
        }
        return Ok((path, vec![]));
    }
    if tool.mode != "project" {
        return Err("Select project or binary launch mode.".into());
    }
    let mut args;
    let program;
    if path.ends_with(".csproj") {
        program = "dotnet";
        args = vec![
            "run".into(),
            "--project".into(),
            path,
            "--configuration".into(),
            tool.configuration.clone(),
        ];
    } else if path.ends_with("Cargo.toml") {
        program = "cargo";
        args = vec!["run".into(), "--manifest-path".into(), path];
        if !tool.target.is_empty() {
            args.extend(["--bin".into(), tool.target.clone()]);
        }
        if tool.configuration != "dev" {
            args.extend(["--profile".into(), tool.configuration.clone()]);
        }
    } else {
        return Err("Choose a .csproj or Cargo.toml for project mode.".into());
    }
    args.extend(tool.build_args.clone());
    args.push("--".into());
    Ok((program.into(), args))
}

async fn pipeline(
    request: &Request,
    directory: &Path,
    reporter: &Reporter,
    mut cancel: watch::Receiver<bool>,
    pid: &AtomicU32,
) -> Result<String, String> {
    let workspace = Path::new(&request.workspace);
    let package = directory
        .join("build.pkg.arc")
        .to_string_lossy()
        .into_owned();
    let source_info = directory
        .join("source-info.json")
        .to_string_lossy()
        .into_owned();
    let (program, mut args) = command_for(&request.compiler, workspace)?;
    args.extend(
        request
            .sources
            .iter()
            .map(|s| resolve(workspace, s).to_string_lossy().into_owned()),
    );
    args.extend([
        "--output".into(),
        package.clone(),
        "--source-info".into(),
        source_info.clone(),
        "--log-level".into(),
        request.log_level.clone(),
        "--package-type".into(),
        request.package_type.clone(),
    ]);
    if request.no_std {
        args.push("--no-std".into());
    }
    if request.format == "ginkgo" {
        args.push("--ginkgo".into());
        if request.compress {
            args.push("--compress".into());
        }
    }
    args.extend(request.compiler.extra_args.clone());
    let status = process(
        "compile",
        program,
        args,
        request,
        reporter,
        &mut cancel,
        pid,
    )
    .await?;
    if status != "passed" {
        return Ok(status);
    }
    if request.action == "compile" {
        return Ok(status);
    }
    if !Path::new(&package).is_file() {
        return Err(
            "Compilation completed without producing a package. Check compiler arguments.".into(),
        );
    }
    if matches!(request.action.as_str(), "decode" | "pipeline") {
        let (program, mut args) = command_for(&request.runtime, workspace)?;
        if request.verbose || request.decode_flags.iter().any(|f| f == "raw") {
            args.push("--verbose".into());
        }
        args.extend(["decode".into(), package.clone()]);
        for flag in &request.decode_flags {
            if !matches!(
                flag.as_str(),
                "descriptor" | "symbols" | "constants" | "instructions" | "raw"
            ) {
                return Err(format!("Unknown decode option: {flag}"));
            }
            args.push(format!("--{flag}"));
        }
        if request.format == "ginkgo" && Path::new(&source_info).is_file() {
            args.extend(["--source-info".into(), source_info]);
        }
        args.extend(request.runtime.extra_args.clone());
        let status = process("decode", program, args, request, reporter, &mut cancel, pid).await?;
        if status != "passed" {
            return Ok(status);
        }
    }
    if matches!(request.action.as_str(), "execute" | "pipeline") {
        let (program, mut args) = command_for(&request.runtime, workspace)?;
        if request.verbose {
            args.push("--verbose".into());
        }
        args.extend([
            "execute".into(),
            package,
            "--repeat".into(),
            request.repeat.max(1).to_string(),
        ]);
        args.extend(request.runtime.extra_args.clone());
        args.push("--".into());
        args.extend(request.args.clone());
        return process(
            "execute",
            program,
            args,
            request,
            reporter,
            &mut cancel,
            pid,
        )
        .await;
    }
    Ok("passed".into())
}

async fn pump<R: AsyncRead + Unpin>(
    mut reader: R,
    reporter: Reporter,
    stage: String,
    stream: &str,
) {
    let mut buffer = [0; 8192];
    let mut pending = Vec::new();
    loop {
        match reader.read(&mut buffer).await {
            Ok(0) => break,
            Ok(count) => {
                pending.extend_from_slice(&buffer[..count]);
                while let Some(end) = pending.iter().position(|b| *b == b'\n') {
                    let line: Vec<_> = pending.drain(..=end).collect();
                    let mut event = Event::new("output", &stage, String::from_utf8_lossy(&line));
                    event.stream = stream.into();
                    reporter.send(event);
                }
                if pending.len() >= 16384 {
                    let mut event = Event::new("output", &stage, String::from_utf8_lossy(&pending));
                    event.stream = stream.into();
                    reporter.send(event);
                    pending.clear();
                }
            }
            Err(error) => {
                let mut event = Event::new(
                    "output",
                    &stage,
                    format!("Unable to read {stream}: {error}\n"),
                );
                event.stream = "stderr".into();
                reporter.send(event);
                break;
            }
        }
    }
    if !pending.is_empty() {
        let mut event = Event::new("output", &stage, String::from_utf8_lossy(&pending));
        event.stream = stream.into();
        reporter.send(event);
    }
}

async fn process(
    stage: &str,
    program: String,
    args: Vec<String>,
    request: &Request,
    reporter: &Reporter,
    cancel: &mut watch::Receiver<bool>,
    process_id: &AtomicU32,
) -> Result<String, String> {
    if *cancel.borrow() {
        return Ok("cancelled".into());
    }
    let mut event = Event::new(
        "stage",
        stage,
        std::iter::once(&program)
            .chain(args.iter())
            .map(|s| {
                if s.chars().any(|c| c.is_whitespace() || c == '"') {
                    format!("{s:?}")
                } else {
                    s.clone()
                }
            })
            .collect::<Vec<_>>()
            .join(" "),
    );
    event.status = "running".into();
    reporter.send(event);
    let started = Instant::now();
    let mut command = Command::new(find_program(&program, &request.environment));
    command
        .args(&args)
        .current_dir(&request.workspace)
        .env("NO_COLOR", "1")
        .env("TERM", "dumb")
        .envs(&request.environment)
        .stdin(Stdio::null())
        .stdout(Stdio::piped())
        .stderr(Stdio::piped())
        .kill_on_drop(true);
    #[cfg(unix)]
    command.process_group(0);
    #[cfg(windows)]
    command.creation_flags(0x08000000);
    let mut child = command.spawn().map_err(|e| {
        format!("Could not launch {program}: {e}. Check the tool path and PATH environment.")
    })?;
    let pid = child.id();
    process_id.store(pid.unwrap_or(0), Ordering::SeqCst);
    let stdout = tokio::spawn(pump(
        child.stdout.take().unwrap(),
        reporter.clone(),
        stage.into(),
        "stdout",
    ));
    let stderr = tokio::spawn(pump(
        child.stderr.take().unwrap(),
        reporter.clone(),
        stage.into(),
        "stderr",
    ));
    let result = tokio::select! {
        status = child.wait() => Some(status.map_err(|e| e.to_string())?),
        _ = cancel.changed() => {
            terminate_tree(pid);
            let _ = child.kill().await;
            None
        }
    };
    process_id.store(0, Ordering::SeqCst);
    // Stop descendants which inherited the output pipes, even after their parent exits.
    terminate_tree(pid);
    let stdout_abort = stdout.abort_handle();
    let stderr_abort = stderr.abort_handle();
    let _ = tokio::time::timeout(std::time::Duration::from_secs(2), async {
        let _ = tokio::join!(stdout, stderr);
    })
    .await;
    stdout_abort.abort();
    stderr_abort.abort();
    let mut event = Event::new("stage", stage, "");
    event.duration_ms = started.elapsed().as_millis() as u64;
    event.status = match result {
        Some(status) if status.success() => "passed",
        Some(_) => "failed",
        None => "cancelled",
    }
    .into();
    event.code = result.and_then(|s| s.code());
    let status = event.status.clone();
    reporter.send(event);
    Ok(status)
}

fn terminate_tree(pid: Option<u32>) {
    let Some(pid) = pid.filter(|pid| *pid != 0) else {
        return;
    };
    #[cfg(unix)]
    unsafe {
        libc::kill(-(pid as i32), libc::SIGKILL);
    }
    #[cfg(windows)]
    {
        use std::os::windows::process::CommandExt;
        let _ = std::process::Command::new("taskkill")
            .args(["/PID", &pid.to_string(), "/T", "/F"])
            .creation_flags(0x08000000)
            .output();
    }
}

// Finder-launched applications often lack the user toolchain paths.
fn find_program(program: &str, environment: &HashMap<String, String>) -> PathBuf {
    if Path::new(program).components().count() > 1 {
        return PathBuf::from(program);
    }
    let path = environment
        .get("PATH")
        .cloned()
        .or_else(|| std::env::var("PATH").ok())
        .unwrap_or_default();
    let mut directories: Vec<_> = std::env::split_paths(&path).collect();
    if let Some(home) = std::env::var_os("HOME").or_else(|| std::env::var_os("USERPROFILE")) {
        directories.push(PathBuf::from(&home).join(".cargo/bin"));
        directories.push(PathBuf::from(home).join(".dotnet"));
    }
    directories.extend(
        [
            "/opt/homebrew/bin",
            "/usr/local/bin",
            "/usr/local/share/dotnet",
        ]
        .map(PathBuf::from),
    );
    #[cfg(windows)]
    if let Some(program_files) = std::env::var_os("ProgramFiles") {
        directories.push(PathBuf::from(program_files).join("dotnet"));
    }
    let name = if cfg!(windows) && !program.ends_with(".exe") {
        format!("{program}.exe")
    } else {
        program.to_string()
    };
    directories
        .into_iter()
        .map(|directory| directory.join(&name))
        .find(|candidate| candidate.is_file())
        .unwrap_or_else(|| PathBuf::from(program))
}

#[tauri::command]
pub async fn export_run(path: String, content: String) -> Result<(), String> {
    tokio::fs::write(PathBuf::from(path), content)
        .await
        .map_err(|e| e.to_string())
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn project_commands_keep_arguments_separate() {
        let workspace = Path::new(env!("CARGO_MANIFEST_DIR"))
            .ancestors()
            .nth(3)
            .unwrap();
        let mut tool = Tool {
            mode: "project".into(),
            path: "src/compiler/Executables/Circle/Arc.Compiler.Circle.csproj".into(),
            target: String::new(),
            configuration: "Debug".into(),
            build_args: vec!["--no-restore".into()],
            extra_args: vec![],
        };
        let (program, args) = command_for(&tool, workspace).unwrap();
        assert_eq!(program, "dotnet");
        assert_eq!(
            &args[3..],
            &["--configuration", "Debug", "--no-restore", "--"]
        );
        tool.path = "src/runtime/Cargo.toml".into();
        tool.target = "arc_cli".into();
        tool.configuration = "release".into();
        tool.build_args.clear();
        let (program, args) = command_for(&tool, workspace).unwrap();
        assert_eq!(program, "cargo");
        assert_eq!(
            &args[3..],
            &["--bin", "arc_cli", "--profile", "release", "--"]
        );
    }

    #[test]
    fn run_directory_only_accepts_run_identifiers() {
        let root = Path::new("/cache/runs");
        assert_eq!(
            run_directory(root, "a1b2-c3").unwrap(),
            root.join("a1b2-c3")
        );
        assert!(run_directory(root, "../outside").is_err());
        assert!(run_directory(root, "").is_err());
    }
    #[cfg(unix)]
    fn fixture(name: &str) -> (Request, Reporter, PathBuf) {
        let directory = std::env::temp_dir().join(format!("stella-{name}-{}", std::process::id()));
        std::fs::create_dir_all(&directory).unwrap();
        let tool = Tool {
            mode: "binary".into(),
            path: "/bin/sh".into(),
            target: String::new(),
            configuration: String::new(),
            build_args: vec![],
            extra_args: vec![],
        };
        let request = Request {
            id: name.into(),
            workspace: directory.to_string_lossy().into_owned(),
            sources: vec![],
            compiler: tool.clone(),
            runtime: tool,
            format: "legacy".into(),
            log_level: "Information".into(),
            package_type: "Executable".into(),
            no_std: false,
            compress: false,
            verbose: false,
            decode_flags: vec![],
            args: vec![],
            repeat: 1,
            action: "pipeline".into(),
            environment: HashMap::new(),
        };
        let reporter = Reporter {
            channel: Channel::new(|_| Ok(())),
            log: Arc::new(Mutex::new(
                File::create(directory.join("events.jsonl")).unwrap(),
            )),
        };
        (request, reporter, directory)
    }

    #[cfg(unix)]
    #[tokio::test]
    async fn captures_both_streams_and_failure_exit_code() {
        let (request, reporter, directory) = fixture("streams");
        let (_sender, mut receiver) = watch::channel(false);
        let pid = AtomicU32::new(0);
        let status = process(
            "execute",
            "/bin/sh".into(),
            vec![
                "-c".into(),
                "printf 'hello'; printf 'failure\\n' >&2; exit 7".into(),
            ],
            &request,
            &reporter,
            &mut receiver,
            &pid,
        )
        .await
        .unwrap();
        assert_eq!(status, "failed");
        let log = std::fs::read_to_string(directory.join("events.jsonl")).unwrap();
        assert!(log.contains("hello") && log.contains("failure"));
        assert!(log.contains("\"code\":7"));
        assert!(log.contains("stdout") && log.contains("stderr"));
        drop(reporter);
        std::fs::remove_dir_all(directory).unwrap();
    }

    #[cfg(unix)]
    #[tokio::test]
    async fn ginkgo_pipeline_attempts_execution_and_reports_runtime_failure() {
        use std::os::unix::fs::PermissionsExt;

        let (mut request, reporter, directory) = fixture("ginkgo-execute");
        request.format = "ginkgo".into();
        let compiler = directory.join("compiler.sh");
        let runtime = directory.join("runtime.sh");
        let attempted = directory.join("execute-attempted");
        std::fs::write(
            &compiler,
            "#!/bin/sh\nwhile [ $# -gt 0 ]; do\n  if [ \"$1\" = --output ]; then shift; : > \"$1\"; exit 0; fi\n  shift\ndone\nexit 2\n",
        )
        .unwrap();
        std::fs::write(
            &runtime,
            format!(
                "#!/bin/sh\nif [ \"$1\" = execute ]; then\n  : > '{}'\n  echo 'Ginkgo execution attempted' >&2\n  exit 17\nfi\nexit 0\n",
                attempted.display()
            ),
        )
        .unwrap();
        for path in [&compiler, &runtime] {
            std::fs::set_permissions(path, std::fs::Permissions::from_mode(0o755)).unwrap();
        }
        request.compiler.path = compiler.to_string_lossy().into_owned();
        request.runtime.path = runtime.to_string_lossy().into_owned();
        let (_sender, receiver) = watch::channel(false);
        let pid = AtomicU32::new(0);

        let status = pipeline(
            &request,
            &directory,
            &reporter,
            receiver,
            &pid,
        )
        .await
        .unwrap();

        assert_eq!(status, "failed");
        assert!(attempted.is_file());
        let log = std::fs::read_to_string(directory.join("events.jsonl")).unwrap();
        assert!(log.contains("Ginkgo execution attempted"));
        assert!(log.contains("\"stage\":\"execute\""));
        assert!(log.contains("\"code\":17"));
        drop(reporter);
        std::fs::remove_dir_all(directory).unwrap();
    }

    #[cfg(unix)]
    #[tokio::test]
    async fn cancels_the_process_group_and_stops_dependent_stages() {
        let (request, reporter, directory) = fixture("cancel");
        let (sender, mut receiver) = watch::channel(false);
        let pid = AtomicU32::new(0);
        let start = Instant::now();
        let (result, _) = tokio::join!(
            process(
                "compile",
                "/bin/sh".into(),
                vec!["-c".into(), "sleep 20 & echo $! > child.pid; wait".into()],
                &request,
                &reporter,
                &mut receiver,
                &pid
            ),
            async {
                tokio::time::sleep(std::time::Duration::from_millis(100)).await;
                sender.send(true).unwrap();
            }
        );
        assert_eq!(result.unwrap(), "cancelled");
        assert!(start.elapsed().as_secs() < 3);
        assert_eq!(pid.load(Ordering::SeqCst), 0);
        assert!(!directory.join("build.pkg.arc").exists());
        drop(reporter);
        std::fs::remove_dir_all(directory).unwrap();
    }
}
