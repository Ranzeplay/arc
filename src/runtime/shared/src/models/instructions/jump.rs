use std::fmt::Debug;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct JumpInstruction {
    pub jump_offset: i64,
}

impl Debug for JumpInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "L:{}", self.jump_offset)
    }
}

impl DecodableInstruction<JumpInstruction> for JumpInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(JumpInstruction, usize)> {
        let jump_offset = i64::from_le_bytes(stream[1..9].try_into().unwrap());
        let length = 9;

        Some((
            JumpInstruction {
                jump_offset,
            },
            length,
        ))
    }
}
