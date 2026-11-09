use arc_cmdec::Cmdec;
use arc_shared::ginkgo::SourceInfo;
use arc_shared::models::options::cmdec_options::CmdecOptions;
use arc_shared::models::package::Package;
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
    if arc_cmdec::ginkgo::is_ginkgo(&stream) {
        let source_info = match source_path.map(read_source_info).transpose() {
            Ok(info) => info,
            Err(err) => {
                error!("Failed to read Ginkgo source information: {}", err);
                return false;
            }
        };
        Cmdec::decode_ginkgo(&stream, &opt, source_info.as_ref()).is_some()
    } else {
        Cmdec::decode(&stream, opt).is_some()
    }
}

fn read_source_info(path: &Path) -> Result<SourceInfo, String> {
    let bytes = std::fs::read(path).map_err(|e| e.to_string())?;
    let info: SourceInfo = serde_json::from_slice(&bytes).map_err(|e| e.to_string())?;
    if info.instruction_set != "arc-ginkgo" {
        return Err(format!(
            "Unexpected instruction set: {}",
            info.instruction_set
        ));
    }
    Ok(info)
}

pub fn call_cmdec_decoder(opt: CmdecOptions) -> Option<Package> {
    let stream = match std::fs::read(&opt.path) {
        Ok(stream) => stream,
        Err(err) => {
            error!("Failed to read {}: {}", opt.path.display(), err);
            return None;
        }
    };
    if arc_cmdec::ginkgo::is_ginkgo(&stream) {
        error!("Ginkgo execution is not supported; use the decode command to inspect this package");
        return None;
    }
    Cmdec::decode(&stream, opt)
}
