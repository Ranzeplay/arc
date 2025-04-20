use std::fmt::Debug;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Copy, Clone)]
pub struct ConditionalJumpInstruction {
    pub jump_offset: i64,
}

impl Debug for ConditionalJumpInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "L:{}", self.jump_offset)
    }
}

impl DecodableInstruction<ConditionalJumpInstruction> for ConditionalJumpInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(ConditionalJumpInstruction, usize)> {
        let jump_offset = i64::from_le_bytes(stream[1..9].try_into().unwrap());
        let length = 9;

        Some((
            ConditionalJumpInstruction {
                jump_offset,
            },
            length,
        ))
    }
}
