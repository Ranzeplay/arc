use crate::command_line_options::Args;
use crate::dispatcher::cmdec::call_cmdec_decoder;
use crate::dispatcher::execute::execute;
use clap::Parser;
use log::{debug, error, info};
use std::process::exit;

mod command_line_options;
mod dispatcher;

fn main() {
    let args = Args::parse();

    let mut log_builder = colog::default_builder();
    if args.verbose {
        log_builder.filter(None, log::LevelFilter::Debug);
    }
    log_builder.init();

    if args.version {
        println!("Arc Runtime CLI v{}", env!("CARGO_PKG_VERSION"));
    }

    match args.subcommand {
        Some(subcommand) => match subcommand {
            command_line_options::Subcommands::Decode(decode_command) => {
                info!("Decoding package content");
                call_cmdec_decoder(&decode_command.path, true, args.verbose);
            }
            command_line_options::Subcommands::Execute(execute_command) => {
                debug!("Executing package");
                let package = call_cmdec_decoder(&execute_command.path, false, args.verbose);
                if package.is_none() {
                    error!("Failed to decode package");
                    exit(1);
                }

                execute(package.unwrap(), args.verbose, execute_command.repeat);
            }
        },
        None => {}
    }
}
