use std::process::exit;
use crate::command_line_options::Args;
use crate::dispatcher::cmdec::call_cmdec_decoder;
use clap::Parser;
use executor::launch;
use log::{debug, error, info};

mod command_line_options;
mod dispatcher;

fn main() {
    let args = Args::parse();

    let mut log_builder = colog::default_builder();
    if args.verbose {
        log_builder.filter(None, log::LevelFilter::Debug);
    }
    log_builder.init();

    if args.path.is_none() {
        error!("No package file provided.");
        exit(0xffff);
    }

    let path = args.path.clone().unwrap();

    if args.decode {
        info!("Decoding package file: {:?}", &path);
    }
    let package = call_cmdec_decoder(&path, args.decode, args.verbose);
    if package.is_none() {
        error!("Error decoding package file.");
        exit(0xffff);
    }
    let package = package.unwrap();

    if !args.decode {
        let result = launch(package, args.verbose);
        if let Err(e) = result {
            error!("Error: {}", e);
            exit(0xffff);
        } else if let Ok(i) = result {
            debug!("Program executed gracefully.");
            exit(i);
        } else {
            unreachable!();
        }
    }

    exit(0);
}
