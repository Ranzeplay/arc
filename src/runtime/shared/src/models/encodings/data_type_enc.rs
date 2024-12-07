pub enum Mutability {
    Immutable,
    Mutable,
}

pub enum MemoryStorageType {
    Reference,
    Value,
}

pub struct DataTypeEncoding {
    pub id: usize,
    pub is_array: bool,
    pub allow_none: bool,
    pub mutability: Mutability,
    pub memory_storage_type: MemoryStorageType,
}
