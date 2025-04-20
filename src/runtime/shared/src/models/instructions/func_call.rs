use std::fmt::Debug;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Copy, Clone)]
pub struct FunctionCallInstruction {
    pub function_id: usize,
    pub param_count: u32,
}

impl Debug for FunctionCallInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "F:{:02X} P:{}", self.function_id, self.param_count)
    }
}

impl DecodableInstruction<FunctionCallInstruction> for FunctionCallInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(FunctionCallInstruction, usize)> {
        let function_id = usize::from_le_bytes(stream[1..9].try_into().unwrap());
        let param_count = u32::from_le_bytes(stream[9..13].try_into().unwrap());

        let length = 13;

        Some((
            FunctionCallInstruction {
                function_id,
                param_count,
            },
            length,
        ))
    }
}
