pub enum Mutability {
    Immutable,
    Mutable,
}

#[derive(Debug)]
pub enum MemoryStorageType {
    Reference,
    Value,
}

pub struct DataTypeEncoding {
    pub type_id: usize,
    pub is_array: bool,
    pub mutability: Mutability,
    pub memory_storage_type: MemoryStorageType,
}

impl DataTypeEncoding {
    pub fn from_u8(stream: &[u8]) -> (DataTypeEncoding, usize) {
        let mut pos = 0;

        let memory_storage_type = match stream[pos] {
            0x01 => MemoryStorageType::Value,
            0x00 => MemoryStorageType::Reference,
            _ => unreachable!(),
        };
        pos += 1;

        let is_array = stream[pos] == 0x01;
        pos += 1;

        let mutability = match stream[pos] {
            0x01 => Mutability::Mutable,
            0x00 => Mutability::Immutable,
            _ => unreachable!(),
        };
        pos += 1;

        let type_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;

        (
            DataTypeEncoding {
                type_id,
                is_array,
                mutability,
                memory_storage_type,
            },
            pos,
        )
    }
}
