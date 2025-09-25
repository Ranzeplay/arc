use std::collections::HashMap;
use std::fmt::Display;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct ProjectInfo {
    pub domain: String,
    pub name: String,
    pub version: String,
    pub description: Option<String>,
    pub dependencies: Option<HashMap<String, DependencyInfo>>,
    pub license: Option<String>,
    pub authors: Option<Vec<String>>,
    pub build: BuildConfig,
}

#[derive(Serialize, Deserialize)]
pub enum BuildTargetType {
    Library,
    Executable,
}

impl Display for BuildTargetType {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            BuildTargetType::Library => write!(f, "Library"),
            BuildTargetType::Executable => write!(f, "Executable"),
        }
    }
}

#[derive(Serialize, Deserialize)]
pub struct DependencyInfo {
    pub version: String,
    pub source_type: DependencySourceType,
    pub source: Option<String>,
}

#[derive(Serialize, Deserialize)]
pub enum DependencySourceType {
    Local,
    Registry,
    Git,
}

#[derive(Serialize, Deserialize)]
pub struct BuildConfig {
    pub target_type: BuildTargetType,
    pub dump_source_info: bool,
}
