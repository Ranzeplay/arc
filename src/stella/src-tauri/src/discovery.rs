use serde::Serialize;
use std::path::{Path, PathBuf};

#[derive(Clone, Serialize, Debug)]
#[serde(rename_all = "camelCase")]
pub struct Candidate {
    pub path: String,
    pub mode: String,
    pub label: String,
    pub target: String,
}

#[derive(Serialize)]
#[serde(rename_all = "camelCase")]
pub struct Defaults {
    workspace: String,
    source: String,
    compiler: String,
    runtime: String,
}

pub fn resolve(workspace: &Path, path: &str) -> PathBuf {
    let path = Path::new(path);
    if path.is_absolute() {
        path.to_path_buf()
    } else {
        workspace.join(path)
    }
}

#[tauri::command]
pub fn bootstrap() -> Defaults {
    let mut starts = vec![];
    if let Ok(cwd) = std::env::current_dir() {
        starts.push(cwd);
    }
    if let Ok(exe) = std::env::current_exe() {
        starts.push(exe);
    }
    let workspace = starts
        .iter()
        .flat_map(|p| p.ancestors())
        .find(|p| p.join("src/compiler").is_dir() && p.join("src/runtime/Cargo.toml").is_file())
        .map(Path::to_path_buf)
        .unwrap_or_else(|| std::env::current_dir().unwrap_or_default());
    Defaults {
        source: "examples/helloWorld.script.arc".into(),
        compiler: "src/compiler/Executables/Circle/Arc.Compiler.Circle.csproj".into(),
        runtime: "src/runtime/Cargo.toml".into(),
        workspace: workspace.to_string_lossy().into_owned(),
    }
}

// Bounded traversal also includes build output directories, which Git normally ignores.
fn walk(
    root: &Path,
    depth: usize,
    files: &mut Vec<PathBuf>,
    visited: &mut usize,
) -> Result<(), String> {
    if depth > 12 {
        return Ok(());
    }
    for entry in std::fs::read_dir(root).map_err(|e| format!("{}: {e}", root.display()))? {
        let entry = entry.map_err(|e| e.to_string())?;
        *visited += 1;
        if *visited > 50_000 {
            return Err("Search exceeded 50,000 entries. Select a narrower directory.".into());
        }
        let kind = entry.file_type().map_err(|e| e.to_string())?;
        if kind.is_symlink() {
            continue;
        }
        let path = entry.path();
        if kind.is_dir() {
            let name = entry.file_name();
            if matches!(
                name.to_str(),
                Some(
                    "node_modules"
                        | ".git"
                        | ".idea"
                        | ".next"
                        | "obj"
                        | "deps"
                        | "incremental"
                        | "build"
                        | ".fingerprint"
                )
            ) {
                continue;
            }
            walk(&path, depth + 1, files, visited)?;
        } else if kind.is_file() {
            files.push(path);
        }
    }
    Ok(())
}

fn files_at(path: &Path) -> Result<Vec<PathBuf>, String> {
    if path.is_file() {
        return Ok(vec![path.to_path_buf()]);
    }
    let mut files = vec![];
    walk(path, 0, &mut files, &mut 0)?;
    files.sort();
    Ok(files)
}

#[tauri::command]
pub async fn discover(
    workspace: String,
    path: String,
    kind: String,
) -> Result<Vec<Candidate>, String> {
    tauri::async_runtime::spawn_blocking(move || {
        discover_at(&resolve(Path::new(&workspace), &path), &kind)
    })
    .await
    .map_err(|e| e.to_string())?
}

fn discover_at(root: &Path, kind: &str) -> Result<Vec<Candidate>, String> {
    let mut candidates = vec![];
    let search_root = if root.file_name().is_some_and(|name| name == "Cargo.toml") {
        let text = std::fs::read_to_string(root).map_err(|e| e.to_string())?;
        let manifest: toml::Value = toml::from_str(&text).map_err(|e| e.to_string())?;
        if manifest.get("workspace").is_some() {
            root.parent().unwrap_or(root)
        } else {
            root
        }
    } else {
        root
    };
    for path in files_at(search_root)? {
        let name = path
            .file_name()
            .and_then(|s| s.to_str())
            .unwrap_or_default();
        let path_text = path.to_string_lossy().into_owned();
        if kind == "compiler" && path.extension().is_some_and(|e| e == "csproj") {
            let text = std::fs::read_to_string(&path).map_err(|e| e.to_string())?;
            if regex::Regex::new(r"(?i)<OutputType>\s*(Exe|WinExe)\s*</OutputType>")
                .unwrap()
                .is_match(&text)
            {
                candidates.push(Candidate {
                    label: format!("{} · .NET project", name.trim_end_matches(".csproj")),
                    path: path_text,
                    mode: "project".into(),
                    target: String::new(),
                });
            }
        } else if kind == "runtime" && name == "Cargo.toml" {
            let text = std::fs::read_to_string(&path).map_err(|e| e.to_string())?;
            let manifest: toml::Value = toml::from_str(&text).map_err(|e| e.to_string())?;
            let package = manifest
                .get("package")
                .and_then(|p| p.get("name"))
                .and_then(|n| n.as_str());
            let mut bins = vec![];
            if let Some(entries) = manifest.get("bin").and_then(|b| b.as_array()) {
                bins.extend(
                    entries
                        .iter()
                        .filter_map(|b| b.get("name").and_then(|n| n.as_str()))
                        .map(str::to_string),
                );
            }
            if path.parent().unwrap().join("src/main.rs").is_file() {
                if let Some(package) = package {
                    if !bins.iter().any(|b| b == package) {
                        bins.push(package.to_string());
                    }
                }
            }
            if let Ok(entries) = std::fs::read_dir(path.parent().unwrap().join("src/bin")) {
                for entry in entries.flatten() {
                    let bin_path = entry.path();
                    if bin_path.extension().is_some_and(|e| e == "rs")
                        || bin_path.join("main.rs").is_file()
                    {
                        if let Some(stem) = bin_path.file_stem().and_then(|s| s.to_str()) {
                            bins.push(stem.to_string());
                        }
                    }
                }
            }
            bins.sort();
            bins.dedup();
            for bin in bins {
                candidates.push(Candidate {
                    label: format!("{bin} · Cargo project"),
                    path: path_text.clone(),
                    mode: "project".into(),
                    target: bin,
                });
            }
        } else {
            let compiler_name = matches!(
                name,
                "Arc.Compiler.Circle" | "Arc.Compiler.Circle.exe" | "Arc.Compiler.Circle.dll"
            ) && !path
                .components()
                .any(|part| matches!(part.as_os_str().to_str(), Some("ref" | "refint")));
            let runtime_name = matches!(name, "arc_cli" | "arc_cli.exe");
            let dll =
                path.extension().is_some_and(|e| e == "dll") && (compiler_name || root.is_file());
            let executable = is_executable(&path);
            if (root.is_file() && (executable || dll))
                || (kind == "compiler" && compiler_name && (executable || dll))
                || (kind == "runtime" && runtime_name && executable)
            {
                candidates.push(Candidate {
                    label: format!("{name} · binary"),
                    path: path_text,
                    mode: "binary".into(),
                    target: String::new(),
                });
            }
        }
    }
    candidates.sort_by_key(|c| {
        (
            c.mode != "project",
            !c.label.starts_with("arc_cli"),
            c.path.clone(),
        )
    });
    Ok(candidates)
}

fn is_executable(path: &Path) -> bool {
    #[cfg(unix)]
    {
        use std::os::unix::fs::PermissionsExt;
        path.extension().is_none()
            && path
                .metadata()
                .is_ok_and(|m| m.permissions().mode() & 0o111 != 0)
    }
    #[cfg(windows)]
    {
        path.extension()
            .is_some_and(|e| e.eq_ignore_ascii_case("exe"))
    }
    #[cfg(not(any(unix, windows)))]
    {
        false
    }
}

#[tauri::command]
pub async fn source_files(workspace: String, path: String) -> Result<Vec<String>, String> {
    tauri::async_runtime::spawn_blocking(move || {
        let path = resolve(Path::new(&workspace), &path);
        let files = files_at(&path)?;
        let sources: Vec<_> = files
            .into_iter()
            .filter(|p| p.to_string_lossy().ends_with(".script.arc"))
            .map(|p| p.to_string_lossy().into_owned())
            .collect();
        if sources.is_empty() {
            return Err("No .script.arc files found at this path.".into());
        }
        Ok(sources)
    })
    .await
    .map_err(|e| e.to_string())?
}

#[tauri::command]
pub async fn read_source(workspace: String, path: String) -> Result<String, String> {
    let path = resolve(Path::new(&workspace), &path);
    let metadata = tokio::fs::metadata(&path)
        .await
        .map_err(|e| e.to_string())?;
    if metadata.len() > 4 * 1024 * 1024 {
        return Err("Source preview is limited to 4 MB.".into());
    }
    tokio::fs::read_to_string(path)
        .await
        .map_err(|e| e.to_string())
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn finds_projects_and_output_layouts() {
        let root = std::env::temp_dir().join(format!("stella-discovery-{}", std::process::id()));
        let dotnet = root.join("compiler/bin/Debug/net11.0");
        let rust = root.join("runtime/target/aarch64-apple-darwin/release");
        std::fs::create_dir_all(&dotnet).unwrap();
        std::fs::create_dir_all(&rust).unwrap();
        std::fs::create_dir_all(root.join("runtime/cli/src")).unwrap();
        std::fs::write(
            root.join("compiler/Circle.csproj"),
            "<Project><PropertyGroup><OutputType>Exe</OutputType></PropertyGroup></Project>",
        )
        .unwrap();
        std::fs::write(dotnet.join("Arc.Compiler.Circle.dll"), "").unwrap();
        std::fs::write(
            root.join("runtime/cli/Cargo.toml"),
            "[package]\nname = 'arc_cli'\nversion = '0.1.0'",
        )
        .unwrap();
        std::fs::write(root.join("runtime/cli/src/main.rs"), "").unwrap();
        let compiler = discover_at(&root, "compiler").unwrap();
        assert_eq!(compiler.len(), 2);
        assert_eq!(compiler[0].mode, "project");
        std::fs::write(
            root.join("runtime/Cargo.toml"),
            "[workspace]\nmembers = ['cli']",
        )
        .unwrap();
        let runtime = discover_at(&root.join("runtime/Cargo.toml"), "runtime").unwrap();
        assert_eq!(runtime[0].target, "arc_cli");
        std::fs::remove_dir_all(root).unwrap();
    }
}
