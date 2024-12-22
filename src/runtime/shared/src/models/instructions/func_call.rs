use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct FunctionCallInstruction {
    pub function_id: usize,
    pub param_count: usize,
}

impl DecodableInstruction<FunctionCallInstruction> for FunctionCallInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(FunctionCallInstruction, usize)> {
        let function_id = usize::from_le_bytes(stream[1..9].try_into().unwrap());
        let param_count = usize::from_le_bytes(stream[9..17].try_into().unwrap());

        let length = 17;

        Some((
            FunctionCallInstruction {
                function_id,
                param_count,
            },
            length,
        ))
    }
}
