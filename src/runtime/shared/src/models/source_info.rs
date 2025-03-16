use std::collections::HashMap;
use serde::Deserialize;

#[derive(Debug, Clone, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct SourceInfo {
    pub uid: usize,
    pub symbol_mapping: HashMap<usize, String>,
    pub function_data_slot_mapping: HashMap<usize, HashMap<usize, String>>,
}
