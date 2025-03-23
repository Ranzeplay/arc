use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct ReturnFunctionInstruction {
    #[allow(dead_code)]
    pub with_value: bool,
}

impl DecodableInstruction<ReturnFunctionInstruction> for ReturnFunctionInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(ReturnFunctionInstruction, usize)> {
        let with_value = stream[1] == 0x01;

        let length = 1 + 1;

        Some((
            ReturnFunctionInstruction {
                with_value,
            },
            length,
        ))
    }
}
