use std::fmt::Debug;
use crate::models::encodings::data_type_enc::MemoryStorageType;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct DeclInstruction {
    pub data_type_id: usize,
    pub is_array: bool,
    pub memory_storage_type: MemoryStorageType,
}

impl Debug for DeclInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        let mst = match self.memory_storage_type {
            MemoryStorageType::Value => 'V',
            MemoryStorageType::Reference => 'R',
        };

        let is_array = if self.is_array { 'A' } else { 'S' };

        write!(
            f,
            "{}/{} {}",
            mst, is_array, self.data_type_id
        )
    }
}

impl DecodableInstruction<DeclInstruction> for DeclInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(DeclInstruction, usize)> {
        let memory_storage_type = if stream[1] == 0x01 {
            MemoryStorageType::Value
        } else {
            MemoryStorageType::Reference
        };
        let is_array = stream[2] == 0x01;
        let data_type_id = usize::from_le_bytes(stream[3..3 + 8].try_into().unwrap());

        let length = 3 + 8;

        Some((
            DeclInstruction {
                data_type_id,
                is_array,
                memory_storage_type,
            },
            length,
        ))
    }
}
