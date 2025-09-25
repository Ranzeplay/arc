use std::io::stdout;
use clap::Parser;
use crossterm::execute;
use crossterm::style::{Print, ResetColor};
use crate::commands::init::init_project;
use crate::models::cli_options::{CommandOptions, Commands};

mod commands;
mod models;

fn main() -> anyhow::Result<()> {
    execute!(stdout(), ResetColor)?;

    execute!(
        stdout(),
        Print("Welcome to Arc CLI!"),
        ResetColor
    )?;

    let options = CommandOptions::parse();

    match options.subcommands {
        Commands::Init(init_options) => {
            init_project(init_options)?;
        }
    }

    Ok(())
}
