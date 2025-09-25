use crate::models::cli_options::InitSubcommandOptions;
use crate::models::project_info::{BuildConfig, BuildTargetType, ProjectInfo};
use crossterm::execute;
use crossterm::style::{Color, Print, ResetColor, SetForegroundColor};
use inquire::validator::Validation;
use inquire::{Select, Text};
use std::fs;
use std::io::stdout;
use regex::Regex;

macro_rules! prompt_match_regex {
    ($prompt:expr, $default:expr, $regex:expr, $error_message:expr) => {
        {
            let validator = |input: &str| {
                if input.trim().is_empty() {
                    return Ok(Validation::Valid);
                }
                if $regex.is_match(input) {
                    Ok(Validation::Valid)
                } else {
                    Ok(Validation::Invalid($error_message.into()))
                }
            };
            match Text::new($prompt)
                .with_default($default)
                .with_validator(validator)
                .prompt()
            {
                Ok(input) if !input.trim().is_empty() => input,
                _ => $default.to_string(),
            }
        }
    };
}

pub fn init_project(options: InitSubcommandOptions) -> anyhow::Result<()> {
    execute!(
        stdout(),
        SetForegroundColor(Color::Green),
        Print("Creating project at `"),
        SetForegroundColor(Color::Cyan),
        Print(options.path.to_str().unwrap_or(".")),
        SetForegroundColor(Color::Green),
        Print("`\r\n"),
        ResetColor
    )?;

    let dir_creation_result = fs::create_dir_all(&options.path);
    if dir_creation_result.is_err() {
        execute!(
            stdout(),
            SetForegroundColor(Color::Red),
            Print("Failed to create project directory"),
            ResetColor
        )?;
        return Err(dir_creation_result.err().unwrap().into());
    }

    // Ask for project details
    let project_root_path = fs::canonicalize(&options.path)?;
    let absolute_dir_name = project_root_path.iter().last().unwrap().to_str();
    let project_name = absolute_dir_name.unwrap_or("unknown");

    let domain = prompt_match_regex!(
            "Project domain:",
            "com.example",
            &Regex::new(r"^[a-zA-Z_][a-zA-Z0-9_]*(\.[a-zA-Z_][a-zA-Z0-9_]*)*$").unwrap(),
            "Domain should be identifiers separated by dots"
        );
        let name = prompt_match_regex!(
            "Project name:",
            project_name,
            &Regex::new(r".+").unwrap(),
            "Name cannot be empty"
        );
        let description = Text::new("Project description (optional):").prompt()?;
        let version = prompt_match_regex!(
            "Project version:",
            "0",
            &Regex::new(r".+").unwrap(),
            "Invalid version format"
        );
        let license = prompt_match_regex!(
            "Project license:",
            "ISC",
            &Regex::new(r".*").unwrap(),
            "Invalid license"
        );
        let authors = prompt_match_regex!(
            "Project authors (comma-separated):",
            "",
            &Regex::new(r"^(\s*|\s*[^,]+(\s*,\s*[^,]+)*)$").unwrap(),
            "Invalid author list format"
        )
        .split(',')
        .map(|s| s.trim().to_string())
        .filter(|s| !s.is_empty())
        .collect::<Vec<_>>();

    let project_type = Select::new(
        "Project type:",
        vec![BuildTargetType::Library, BuildTargetType::Executable],
    )
    .prompt()?;

    let project_info = ProjectInfo {
        domain,
        name,
        version,
        description: None,
        dependencies: None,
        license: match license.trim() {
            "" => None,
            _ => Some(license),
        },
        authors: if authors.is_empty() { None } else { Some(authors) },
        build: BuildConfig {
            target_type: project_type,
            dump_source_info: false,
        },
    };

    // Create project directory structure
    execute!(
        stdout(),
        SetForegroundColor(Color::Green),
        Print("Setting up project structure...\r\n"),
        ResetColor
    )?;

    // Save project info to project.toml
    let project_toml_path = project_root_path.join("project.toml");
    let toml_content = toml::to_string_pretty(&project_info)?;
    fs::write(&project_toml_path, toml_content)?;

    let src_path = project_root_path.join("src");
    fs::create_dir_all(&src_path)?;

    match project_info.build.target_type {
        BuildTargetType::Library => {
            let lib_rs_path = src_path.join("lib.script.arc");
            fs::write(&lib_rs_path, include_str!("../../templates/default_library.script.arc"))?;
        }
        BuildTargetType::Executable => {
            let main_rs_path = src_path.join("main.script.arc");
            fs::write(&main_rs_path, include_str!("../../templates/default_executable.script.arc"))?;
        }
    }

    execute!(
        stdout(),
        SetForegroundColor(Color::Green),
        Print("Project created successfully!\r\n"),
        ResetColor
    )?;

    Ok(())
}
