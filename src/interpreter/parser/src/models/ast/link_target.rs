use std::path::PathBuf;

pub enum LinkTarget {
    External(ExternalLink),
    Path(PathBuf),
}

pub struct ExternalLink {
    pub package_name: String,
    pub namespace: String,
}
