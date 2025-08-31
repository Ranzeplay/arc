use std::fmt::Debug;
use crate::models::encodings::sized_array_enc::SizedArrayEncoding;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Clone)]
pub struct NewObjectInstruction {
    pub type_id: usize,
    pub generic_type_ids: Vec<usize>,
}

impl Debug for NewObjectInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "0x{:016X}", self.type_id)
    }
}

impl DecodableInstruction<NewObjectInstruction> for NewObjectInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(NewObjectInstruction, usize)> {
        let mut pos = 1;
        let type_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;

        let (generic_type_ids, generic_types_len) = SizedArrayEncoding::with_usize_data(stream[pos..].try_into().unwrap());
        pos += generic_types_len;

        Some((
            NewObjectInstruction {
                type_id,
                generic_type_ids,
            },
            pos,
        ))
    }
}
