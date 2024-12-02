use clap::Parser;
use crate::command_line_options::Args;

mod command_line_options;

fn main() {
    let args = Args::parse();

    println!("Hello, world!");
}
