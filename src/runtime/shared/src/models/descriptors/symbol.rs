use std::collections::HashMap;
use crate::models::encodings::data_type_enc::DataTypeEncoding;
use std::fmt::Debug;
use std::rc::Rc;

#[derive(Debug, Clone)]
pub struct SymbolDescriptor {
    pub id: usize,
    pub value: Symbol,
}

#[derive(Debug, Clone)]
pub enum Symbol {
    DataType(Rc<DataTypeSymbol>),
    Function(Rc<FunctionSymbol>),
    Group(Rc<GroupSymbol>),
    GroupField(Rc<GroupFieldSymbol>),
    Namespace(Rc<NamespaceSymbol>),
    Annotation(Rc<AnnotationSymbol>),
}

pub enum DataTypeSymbol {
    BaseType,
    ComplexType(ComplexTypeSymbol),
}

pub struct ComplexTypeSymbol {
    pub group_id: usize,
}

impl Debug for DataTypeSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match &self {
            DataTypeSymbol::ComplexType(dt) => writeln!(f, "[DT] Ref 0x{:016X}", dt.group_id),
            DataTypeSymbol::BaseType => writeln!(f, "[DT]")
        }
    }
}

#[derive(Clone)]
pub struct FunctionSymbol {
    pub entry_pos: usize,
    pub block_length: usize,
    pub return_value_descriptor: Rc<DataTypeEncoding>,
    pub annotation_ids: Vec<usize>,
    pub parameter_descriptors: Vec<Rc<DataTypeEncoding>>,
    pub data_count: usize
}

impl Debug for FunctionSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[FN] at {}(+{}) A:{} D:{}", self.entry_pos, self.block_length, self.annotation_ids.len(), self.data_count)
    }
}

#[derive(Clone)]
pub struct GroupSymbol {
    pub field_ids: Vec<usize>,
    pub constructor_ids: Vec<usize>,
    pub destructor_ids: Vec<usize>,
    pub function_ids: Vec<usize>,
    pub sub_group_ids: Vec<usize>,
    pub annotation_ids: Vec<usize>,
}

impl Debug for GroupSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[GP] {}/{}/{}/{}/{}",
                 self.field_ids.len(),
                 self.function_ids.len(),
                 self.constructor_ids.len(),
                 self.destructor_ids.len(),
                 self.sub_group_ids.len()
        )
    }
}

pub struct GroupFieldSymbol {
    pub value_descriptor: DataTypeEncoding
}

impl Debug for GroupFieldSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[GF]")
    }
}

pub struct NamespaceSymbol {
}

impl Debug for NamespaceSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[NS]")
    }
}

pub struct AnnotationSymbol {
    pub group_id: usize,
}

impl Debug for AnnotationSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "[AN] Ref 0x{:016X}", self.group_id)
    }
}

#[derive(Clone)]
pub struct SymbolTable {
    pub symbols: HashMap<usize, SymbolDescriptor>,
}

impl Debug for SymbolTable {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "=== Symbol table")?;
        for symbol in self.symbols.values() {
            write!(f, "0x{:016X}: ", symbol.id)?;
            match &symbol.value {
                Symbol::DataType(data_type) => write!(f, "{:?}", data_type)?,
                Symbol::Function(function) => write!(f, "{:?}", function)?,
                Symbol::Group(group) => write!(f, "{:?}", group)?,
                Symbol::GroupField(group_field) => write!(f, "{:?}", group_field)?,
                Symbol::Namespace(namespace) => write!(f, "{:?}", namespace)?,
                Symbol::Annotation(annotation) => write!(f, "{:?}", annotation)?,
            }
        }
        Ok(())
    }
}
