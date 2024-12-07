use std::fmt::Debug;

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

pub struct DataTypeSymbol {
    pub signature: String,
}

impl Debug for DataTypeSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "Data type: {}", self.signature)
    }
}

pub struct FunctionSymbol {
    pub signature: String,
    pub entry_pos: usize,
}

impl Debug for FunctionSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "Function: {} at {}", self.signature, self.entry_pos)
    }
}

pub struct GroupSymbol {
    pub signature: String,
}

impl Debug for GroupSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "Group: {}", self.signature)
    }
}

pub struct GroupFieldSymbol {
    pub signature: String,
}

impl Debug for GroupFieldSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "Group field: {}", self.signature)
    }
}

pub struct NamespaceSymbol {
    pub signature: String,
}

impl Debug for NamespaceSymbol {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "Namespace: {}", self.signature)
    }
}

pub struct SymbolTable {
    pub symbols: Vec<SymbolDescriptor>,
}

impl Debug for SymbolTable {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "=== SymbolTable")?;
        for symbol in &self.symbols {
            write!(f, "{}: ", symbol.id)?;
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
