use clap::Parser;
use executor::launch;
use crate::command_line_options::Args;
use crate::dispatcher::cmdec::call_cmdec;

mod command_line_options;
mod dispatcher;

fn main() {
    let args = Args::parse();

    if args.decode {
        println!("Decoding package file: {:?}", args.path);
        let package = call_cmdec(&args.path.unwrap());
        launch(package, args.verbose).unwrap();
    }
}
