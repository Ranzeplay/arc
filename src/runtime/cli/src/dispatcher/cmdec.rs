use std::path::PathBuf;
use cmdec::Cmdec;

pub fn call_cmdec(path: &PathBuf){
    let stream = std::fs::read(path).unwrap();
    Cmdec::decode(&stream);
}
