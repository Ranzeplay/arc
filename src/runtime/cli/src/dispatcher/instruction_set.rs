use arc_shared::ginkgo::{Package as GinkgoPackage, SourceInfo};
use arc_shared::models::options::cmdec_options::CmdecOptions;
use arc_shared::models::options::launch_options::LaunchOptions;
use arc_shared::models::package::Package as LegacyPackage;
use std::path::Path;
use std::rc::Rc;

trait InstructionSet {
    type Package;

    fn matches(bytes: &[u8]) -> bool;
    fn decode(
        bytes: &[u8],
        options: &CmdecOptions,
        source_info_path: Option<&Path>,
    ) -> Result<Self::Package, String>;
    fn execute(
        package: &Self::Package,
        repeat: u32,
        args: Vec<String>,
        verbose: bool,
    ) -> Result<i32, String>;
}

struct LegacyInstructionSet;
struct GinkgoInstructionSet;

impl InstructionSet for LegacyInstructionSet {
    type Package = LegacyPackage;

    fn matches(bytes: &[u8]) -> bool {
        bytes.starts_with(&[0x20, 0x24])
    }

    fn decode(
        bytes: &[u8],
        options: &CmdecOptions,
        _source_info_path: Option<&Path>,
    ) -> Result<Self::Package, String> {
        arc_cmdec::Cmdec::decode(bytes, options.clone())
            .ok_or_else(|| "Failed to decode legacy package".to_string())
    }

    fn execute(
        package: &Self::Package,
        repeat: u32,
        args: Vec<String>,
        verbose: bool,
    ) -> Result<i32, String> {
        let options = LaunchOptions {
            package: Rc::new(package.clone()),
            verbose,
            repeat,
            args,
        };
        Ok(super::execute::execute(options))
    }
}

impl InstructionSet for GinkgoInstructionSet {
    type Package = GinkgoPackage;

    fn matches(bytes: &[u8]) -> bool {
        arc_cmdec::ginkgo::is_ginkgo(bytes)
    }

    fn decode(
        bytes: &[u8],
        options: &CmdecOptions,
        source_info_path: Option<&Path>,
    ) -> Result<Self::Package, String> {
        let source_info = source_info_path
            .map(read_source_info)
            .transpose()
            .map_err(|error| format!("Failed to read Ginkgo source information: {}", error))?;
        arc_cmdec::Cmdec::decode_ginkgo(bytes, options, source_info.as_ref())
            .ok_or_else(|| "Failed to decode Ginkgo package".to_string())
    }

    fn execute(
        package: &Self::Package,
        repeat: u32,
        _args: Vec<String>,
        _verbose: bool,
    ) -> Result<i32, String> {
        let mut exit_code = 0;
        for _ in 0..repeat {
            exit_code = arc_executor::ginkgo::pipeline::entry(package)? as i32;
            if exit_code != 0 {
                break;
            }
        }
        Ok(exit_code)
    }
}

pub enum DecodedPackage {
    Legacy(LegacyPackage),
    Ginkgo(GinkgoPackage),
}

impl DecodedPackage {
    pub fn decode(
        bytes: &[u8],
        options: &CmdecOptions,
        source_info_path: Option<&Path>,
    ) -> Result<Self, String> {
        if GinkgoInstructionSet::matches(bytes) {
            return GinkgoInstructionSet::decode(bytes, options, source_info_path)
                .map(Self::Ginkgo);
        }
        if LegacyInstructionSet::matches(bytes) {
            return LegacyInstructionSet::decode(bytes, options, source_info_path)
                .map(Self::Legacy);
        }
        Err("Unrecognized package format".to_string())
    }

    pub fn execute(&self, repeat: u32, args: Vec<String>, verbose: bool) -> Result<i32, String> {
        match self {
            Self::Legacy(package) => LegacyInstructionSet::execute(package, repeat, args, verbose),
            Self::Ginkgo(package) => GinkgoInstructionSet::execute(package, repeat, args, verbose),
        }
    }
}

fn read_source_info(path: &Path) -> Result<SourceInfo, String> {
    let bytes = std::fs::read(path).map_err(|error| error.to_string())?;
    let info: SourceInfo = serde_json::from_slice(&bytes).map_err(|error| error.to_string())?;
    if info.instruction_set != "arc-ginkgo" {
        return Err(format!(
            "Unexpected instruction set: {}",
            info.instruction_set
        ));
    }
    Ok(info)
}
