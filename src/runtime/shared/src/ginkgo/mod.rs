use serde::Deserialize;
use std::collections::BTreeMap;

#[derive(Debug, Clone)]
pub struct Package {
    pub descriptor: PackageDescriptor,
    pub symbols: BTreeMap<u32, Symbol>,
    pub constant_pools: Vec<BTreeMap<u32, Constant>>,
    pub instructions: Vec<Instruction>,
}

#[derive(Debug, Clone)]
pub struct PackageDescriptor {
    pub package_type: u8,
    pub name: String,
    pub version: i64,
    pub entrypoint_function_id: u32,
    pub data_alignment_length: u64,
    pub root_function_table_entry_position: u64,
    pub root_constant_table_entry_position: u64,
    pub root_group_table_entry_position: u64,
    pub region_table_entry_position: u64,
}

#[derive(Debug, Clone)]
pub struct Declaration {
    pub dimension: i32,
    pub mutable: bool,
    pub type_id: u32,
    pub allow_none: bool,
}

#[derive(Debug, Clone)]
pub enum Symbol {
    Namespace,
    Function(Function),
    GroupField(Declaration),
    DataType(Option<u32>),
    Group {
        field_ids: Vec<u32>,
        lifecycle_functions: Vec<(u8, u32)>,
        function_ids: Vec<u32>,
        subgroup_ids: Vec<u32>,
        annotation_ids: Vec<u32>,
        generic_type_ids: Vec<u32>,
        derivation_type_ids: Vec<u32>,
    },
    Annotation(u32),
    Enum {
        annotation_ids: Vec<u32>,
        member_ids: Vec<u32>,
    },
    EnumMember {
        annotation_ids: Vec<u32>,
    },
    GenericTypeParameter(u32),
}

#[derive(Debug, Clone)]
pub struct Function {
    pub code_offset: usize,
    pub code_length: usize,
    pub return_declaration: Declaration,
    pub parameters: Vec<Declaration>,
    pub annotation_ids: Vec<u32>,
    pub generic_type_ids: Vec<u32>,
    pub register_count: usize,
    pub constant_pool: u32,
}

#[derive(Debug, Clone)]
pub struct Constant {
    pub type_id: u32,
    pub is_array: bool,
    pub raw_value: Vec<u8>,
}

#[derive(Debug, Clone, PartialEq)]
pub enum Operand {
    Register(u16),
    Symbol(u32),
    Constant(u32),
    Integer(i64),
    Float(f64),
    Address(i64),
    Dimension(u16),
}

#[derive(Debug, Clone)]
pub struct Instruction {
    /// Byte offset relative to the instruction section.
    pub offset: usize,
    pub opcode: u8,
    pub mnemonic: &'static str,
    pub operands: Vec<Operand>,
    pub flags: Option<u8>,
    pub raw: Vec<u8>,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct SourceInfo {
    pub instruction_set: String,
    pub symbol_mapping: BTreeMap<u32, String>,
    pub function_sources: BTreeMap<u32, String>,
}
