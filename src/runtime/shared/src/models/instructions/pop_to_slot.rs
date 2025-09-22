use std::fmt::Debug;
use arc_instruction_factory::arc_instruction;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Copy, Clone)]
pub struct PopToSlotInstruction {
    pub slot_id: usize,
}

impl Debug for PopToSlotInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "id:{}", self.slot_id)
    }
}

#[arc_instruction(0x06, "PopS")]
impl DecodableInstruction<PopToSlotInstruction> for PopToSlotInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(PopToSlotInstruction, usize)> {
        let slot_id = usize::from_le_bytes(stream[1..9].try_into().unwrap());
        let length = 9;

        Some((
            PopToSlotInstruction {
                slot_id,
            },
            length,
        ))
    }
}
