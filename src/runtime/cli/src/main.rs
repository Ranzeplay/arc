use crate::command_line_options::Args;
use crate::dispatcher::cmdec::{call_cmdec_decoder, decode};
use crate::dispatcher::execute::execute;
use arc_shared::models::options::cmdec_options::CmdecOptions;
use arc_shared::models::options::launch_options::LaunchOptions;
use clap::Parser;
use log::{debug, error, info};
use mimalloc::MiMalloc;
use std::process::exit;
use std::rc::Rc;

mod command_line_options;
mod dispatcher;

#[global_allocator]
static GLOBAL: MiMalloc = MiMalloc;

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
                let opt = CmdecOptions {
                    path: decode_command.path,
                    print_instructions: decode_command.instructions,
                    print_symbols: decode_command.symbols,
                    print_constants: decode_command.constants,
                    print_package_descriptor: decode_command.descriptor,
                    print_raw_bytes: decode_command.raw,
                    verbose: args.verbose,
                };
                if !decode(opt, decode_command.source_info.as_deref()) {
                    exit(1);
                }
            }
            command_line_options::Subcommands::Execute(execute_command) => {
                debug!("Executing package");
                let bytes = match std::fs::read(&execute_command.path) {
                    Ok(bytes) => bytes,
                    Err(err) => {
                        error!("Failed to read {}: {}", execute_command.path.display(), err);
                        exit(1);
                    }
                };
                if arc_cmdec::ginkgo::is_ginkgo(&bytes) {
                    let package = match arc_cmdec::ginkgo::decode(&bytes) {
                        Ok(package) => package,
                        Err(err) => {
                            error!("Failed to decode Ginkgo package: {}", err);
                            exit(1);
                        }
                    };
                    let mut exit_code = 0;
                    for _ in 0..execute_command.repeat {
                        match arc_executor::ginkgo::pipeline::entry(&package) {
                            Ok(code) => exit_code = code as i32,
                            Err(err) => {
                                error!("Ginkgo execution failed: {}", err);
                                exit(1);
                            }
                        }
                        if exit_code != 0 {
                            break;
                        }
                    }
                    exit(exit_code);
                }
                let opt = CmdecOptions {
                    path: execute_command.path,
                    print_constants: false,
                    print_instructions: false,
                    print_symbols: false,
                    print_raw_bytes: false,
                    print_package_descriptor: false,
                    verbose: args.verbose,
                };
                let package = call_cmdec_decoder(opt);
                if package.is_none() {
                    error!("Failed to decode package");
                    exit(1);
                }

                let opt = LaunchOptions {
                    package: Rc::new(package.unwrap()),
                    verbose: args.verbose,
                    repeat: execute_command.repeat,
                    args: execute_command.args,
                };
                exit(execute(opt));
            }
        },
        None => {}
    }
}
