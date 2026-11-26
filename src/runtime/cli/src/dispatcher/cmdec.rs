use super::instruction_set::DecodedPackage;
use arc_shared::models::options::cmdec_options::CmdecOptions;
use log::error;
use std::path::Path;

pub fn decode(opt: CmdecOptions, source_path: Option<&Path>) -> bool {
    let stream = match std::fs::read(&opt.path) {
        Ok(stream) => stream,
        Err(err) => {
            error!("Failed to read {}: {}", opt.path.display(), err);
            return false;
        }
    };

    match DecodedPackage::decode(&stream, &opt, source_path) {
        Ok(_) => true,
        Err(err) => {
            error!("{}", err);
            false
        }
    }
}
