use std::fmt::Debug;
use crate::models::encodings::data_type_enc::MemoryStorageType;
use crate::models::encodings::sized_array_enc::SizedArrayEncoding;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Clone)]
pub struct DeclInstruction {
    pub data_type_id: usize,
    pub dimension: u32,
    pub memory_storage_type: MemoryStorageType,
    pub generic_type_ids: Vec<usize>,
}

impl Debug for DeclInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        let mst = match self.memory_storage_type {
            MemoryStorageType::Value => 'V',
            MemoryStorageType::Reference => 'R',
        };

        let is_array = if self.dimension > 0 { 'A' } else { 'S' };

        write!(
            f,
            "{}/{} 0x{:016X}",
            mst, is_array, self.data_type_id
        )
    }
}

impl DecodableInstruction<DeclInstruction> for DeclInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(DeclInstruction, usize)> {
        let mut pos = 1;
        let memory_storage_type = MemoryStorageType::from_u8(stream[1]);
        pos += 1;

        let dimension = u32::from_le_bytes(stream[pos..pos + 4].try_into().unwrap());
        pos += 4;

        let data_type_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;

        let (generic_type_ids, generic_types_len) = SizedArrayEncoding::with_usize_data(stream[pos..].try_into().unwrap());
        pos += generic_types_len;

        Some((
            DeclInstruction {
                data_type_id,
                dimension,
                memory_storage_type,
                generic_type_ids,
            },
            pos,
        ))
    }
}
