use arc_cmdec::Cmdec;
use arc_shared::models::options::cmdec_options::CmdecOptions;
use arc_shared::models::package::Package;

pub fn call_cmdec_decoder(opt: CmdecOptions) -> Option<Package> {
    let stream = std::fs::read(&opt.path).unwrap();
    Cmdec::decode(&stream, opt)
}
