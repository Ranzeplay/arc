use std::path::PathBuf;
use cmdec::Cmdec;
use shared::models::package::Package;

pub fn call_cmdec(path: &PathBuf) -> Package {
    let stream = std::fs::read(path).unwrap();
    Cmdec::decode(&stream)
}
