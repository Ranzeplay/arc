use std::path::PathBuf;
use clap::{Parser, Subcommand};

#[derive(Parser, Debug)]
#[command(about = "Arc Runtime CLI", arg_required_else_help(true))]
pub struct Args {
    #[clap(subcommand)]
    pub subcommand: Option<Subcommands>,
    #[clap(short, long, help = "Verbose output")]
    pub verbose: bool,
    #[clap(long, help = "Print version information")]
    pub version: bool,
}

#[derive(Debug, Subcommand)]
pub enum Subcommands {
    Decode(DecodeCommand),
    Execute(ExecuteCommand),
}

#[derive(Parser, Debug)]
#[command(about = "Decode package content but not execute")]
pub struct DecodeCommand {
    #[clap(required = true, help = "Path to the package file")]
    pub path: PathBuf,
    #[clap(short = 'm', long, help = "Source information for the package file")]
    pub source_info: Option<PathBuf>,
    #[clap(short, long, help = "Show constant table", default_value="false")]
    pub constants: bool,
    #[clap(short, long, help = "Show function table", default_value="false")]
    pub symbols: bool,
    #[clap(short, long, help = "Show instructions", default_value="false")]
    pub instructions: bool,
    #[clap(short, long, help = "Show package descriptor", default_value="false")]
    pub descriptor: bool,
    #[clap(short, long, help = "Show raw bytes", default_value="false")]
    pub raw: bool,
}

// Run subcommand
#[derive(Parser, Debug)]
#[command(about = "Execute package")]
pub struct ExecuteCommand {
    #[clap(required = true, help = "Path to the package file")]
    pub path: PathBuf,
    #[clap(short, long, help = "Repeat execution for specific times", default_value("1"))]
    pub repeat: u32,
    #[clap(help = "Arguments to pass to the package")]
    pub args: Vec<String>,
}
