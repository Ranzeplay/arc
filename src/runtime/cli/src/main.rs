use clap::Parser;
use crate::command_line_options::Args;
use crate::dispatcher::cmdec::call_cmdec;

mod command_line_options;
mod dispatcher;

fn main() {
    let args = Args::parse();

    if args.decode {
        println!("Decoding package file: {:?}", args.path);
        call_cmdec(&args.path.unwrap());
    }
}
