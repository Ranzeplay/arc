use std::fmt::Debug;
use arc_instruction_factory::arc_instruction;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Copy, Clone)]
pub struct ReturnInstruction {
    pub with_value: bool,
}

impl Debug for ReturnInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{}", if self.with_value { "value" } else { "empty" })
    }
}

#[arc_instruction(0x35, "FRet")]
impl DecodableInstruction<ReturnInstruction> for ReturnInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(ReturnInstruction, usize)> {
        let with_value = stream[1] == 0x01;
        let length = 2;

        Some((
            ReturnInstruction {
                with_value,
            },
            length,
        ))
    }
}
