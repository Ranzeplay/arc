use std::fs;
use crate::models::options::InitSubcommandOptions;

pub fn init_project(options: InitSubcommandOptions) {
    println!("Creating project at {:?}", options.path);
    fs::create_dir_all(&options.path)
        .expect("Failed to create project directory");

    let src_path = options.path.join("src");
    fs::create_dir(&src_path)
        .expect("Failed to create src directory");

    let main_path = src_path.join("main.script.arc");
    fs::write(&main_path, include_str!("../../templates/default_src_main.script.arc"))
        .expect("Failed to create main script file");

    let config_path = options.path.join("project.toml");
    fs::write(&config_path, include_str!("../../templates/default_proj_conf.toml"))
        .expect("Failed to create project configuration file");

    println!("Project created successfully");
}
