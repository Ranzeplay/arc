use std::fmt::Debug;
use arc_instruction_factory::arc_instruction;
use crate::models::encodings::sized_array_enc::SizedArrayEncoding;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Clone)]
pub struct FunctionCallInstruction {
    pub function_id: usize,
    pub param_count: u32,
    pub generic_type_ids: Vec<usize>,
}

impl Debug for FunctionCallInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "F:{:02X} P:{}", self.function_id, self.param_count)
    }
}

#[arc_instruction(0x36, "FCall")]
impl DecodableInstruction<FunctionCallInstruction> for FunctionCallInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(FunctionCallInstruction, usize)> {
        let mut pos = 1;
        let function_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        let param_count = u32::from_le_bytes(stream[pos..pos + 4].try_into().unwrap());
        pos += 4;

        let (generic_type_ids, generic_types_len) = SizedArrayEncoding::with_usize_data(&stream[13..]);
        pos += generic_types_len;

        Some((
            FunctionCallInstruction {
                function_id,
                param_count,
                generic_type_ids,
            },
            pos,
        ))
    }
}
