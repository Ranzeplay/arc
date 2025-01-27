use clap::Parser;
use crate::commands::init::init_project;
use crate::models::options::{CommandOptions, Commands};

mod commands;
mod models;

fn main() {
    let options = CommandOptions::parse();

    match options.subcommands {
        Commands::Init(init_options) => {
            init_project(init_options);
        }
    }
}
