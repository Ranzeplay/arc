use std::path::PathBuf;

pub struct CmdecOptions {
    pub path: PathBuf,
    pub print_instructions: bool,
    pub print_symbols: bool,
    pub print_constants: bool,
    pub print_raw_bytes: bool,
    pub print_package_descriptor: bool,
    pub verbose: bool
}
