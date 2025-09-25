use std::path::PathBuf;
use clap::{Parser, Subcommand};

#[derive(Parser, Debug)]
#[command(about = "Arc CLI", arg_required_else_help(true))]
pub struct CommandOptions {
    #[clap(subcommand)]
    pub subcommands: Commands,
    #[clap(short, long, help = "Verbose mode")]
    pub verbose: bool
}

#[derive(Parser, Debug)]
#[clap(name = "init", about = "Initialize a new project")]
pub struct InitSubcommandOptions {
    #[arg(required = true, help = "Path to the project directory", default_value = ".")]
    pub path: PathBuf
}

#[derive(Subcommand, Debug)]
pub enum Commands {
    Init(InitSubcommandOptions)
}
