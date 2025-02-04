use std::path::PathBuf;
use cmdec::Cmdec;
use shared::models::package::Package;

pub fn call_cmdec_decoder(path: &PathBuf, print_decoding_result: bool, verbose: bool) -> Package {
    let stream = std::fs::read(path).unwrap();
    Cmdec::decode(&stream, print_decoding_result, verbose)
}
