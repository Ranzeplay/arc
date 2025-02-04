use std::fmt::Debug;
use crate::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};

pub struct SymbolDescriptor {
    pub id: usize,
    pub value: Symbol,
}

pub enum Symbol {
    DataType(DataTypeSymbol),
    Function(FunctionSymbol),
    Group(GroupSymbol),
    GroupField(GroupFieldSymbol),
    Namespace(NamespaceSymbol),
}

pub enum DataTypeSymbol {
    BaseType(String),
    DerivativeType(DerivativeTypeSymbol),
}

pub struct DerivativeTypeSymbol {
    pub signature: String,
    pub group_id: usize,
}

impl Debug for DataTypeSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match &self {
            DataTypeSymbol::DerivativeType(dt) => writeln!(f, "[DT] {} -> 0x{:016X}", dt.signature, dt.group_id),
            DataTypeSymbol::BaseType(bt) => writeln!(f, "[DT] {}", bt)
        }
    }
}

pub struct FunctionSymbol {
    pub signature: String,
    pub entry_pos: usize,
    pub block_length: usize,
    pub return_value_descriptor: DataTypeEncoding,
    pub parameter_descriptors: Vec<DataTypeEncoding>,
}

impl Debug for FunctionSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[FN] {} at {}(+{})", self.signature, self.entry_pos, self.block_length)
    }
}

#[derive(Clone)]
pub struct GroupSymbol {
    pub signature: String,
    pub field_ids: Vec<usize>,
    pub constructor_ids: Vec<usize>,
    pub destructor_ids: Vec<usize>,
    pub function_ids: Vec<usize>,
    pub sub_group_ids: Vec<usize>,
}

impl Debug for GroupSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[GP] {} {}/{}/{}/{}/{}",
                 self.signature,
                 self.field_ids.len(),
                 self.function_ids.len(),
                 self.constructor_ids.len(),
                 self.destructor_ids.len(),
                 self.sub_group_ids.len()
        )
    }
}

pub struct GroupFieldSymbol {
    pub signature: String,
    pub value_descriptor: DataTypeEncoding
}

impl Debug for GroupFieldSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[GF] {}", self.signature)
    }
}

pub struct NamespaceSymbol {
    pub signature: String,
}

impl Debug for NamespaceSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[NS] {}", self.signature)
    }
}

pub struct SymbolTable {
    pub symbols: Vec<SymbolDescriptor>,
}

impl Debug for SymbolTable {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "=== Symbol table")?;
        for symbol in &self.symbols {
            write!(f, "0x{:016X}: ", symbol.id)?;
            match &symbol.value {
                Symbol::DataType(data_type) => write!(f, "{:?}", data_type)?,
                Symbol::Function(function) => write!(f, "{:?}", function)?,
                Symbol::Group(group) => write!(f, "{:?}", group)?,
                Symbol::GroupField(group_field) => write!(f, "{:?}", group_field)?,
                Symbol::Namespace(namespace) => write!(f, "{:?}", namespace)?,
            }
        }
        Ok(())
    }
}
