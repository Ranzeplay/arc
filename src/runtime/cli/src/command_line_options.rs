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
    #[clap(short, long, help = "Path to the package file")]
    pub path: PathBuf,
}

// Run subcommand
#[derive(Parser, Debug)]
#[command(about = "Execute package")]
pub struct ExecuteCommand {
    #[clap(short, long, help = "Path to the package file")]
    pub path: PathBuf,
    #[clap(short, long, help = "Repeat execution for specific times", default_value("1"))]
    pub repeat: u32,
}
