use std::path::PathBuf;
use clap::Parser;

#[derive(Parser, Debug)]
#[command(about = "Arc Runtime CLI", arg_required_else_help(true))]
pub struct Args {
    #[clap(short, long, help = "Verbose output")]
    pub verbose: bool,
    #[clap(long, help = "Prints version information")]
    pub version: bool,
    #[clap(short, long, help = "Only decode package content but not execute", requires("path"))]
    pub decode: bool,
    #[clap(short, long, help = "Path to the package file")]
    pub path: Option<PathBuf>,
    #[clap(short, long, help = "Repeat execution for specific times", requires("path"), default_value("1"))]
    pub repeat: u32,
}
