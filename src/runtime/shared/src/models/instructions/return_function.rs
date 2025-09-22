use std::fmt::Debug;
use arc_instruction_factory::arc_instruction;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Copy, Clone)]
pub struct ReturnFunctionInstruction {
    #[allow(dead_code)]
    pub with_value: bool,
}

impl Debug for ReturnFunctionInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{}", if self.with_value { "value" } else { "empty" })
    }
}

#[arc_instruction(0x1a, "FRet")]
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
