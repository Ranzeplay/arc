use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct ConditionalJumpInstruction {
    pub jump_offset: i64,
}

impl DecodableInstruction<ConditionalJumpInstruction> for ConditionalJumpInstruction {
    fn decode(stream: &[u8], offset: usize, _package: &Package) -> Option<(ConditionalJumpInstruction, usize)> {
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
