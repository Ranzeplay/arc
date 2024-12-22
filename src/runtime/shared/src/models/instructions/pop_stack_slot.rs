use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct PopStackSlotInstruction {
    pub slot_id: usize,
}

impl DecodableInstruction<PopStackSlotInstruction> for PopStackSlotInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(PopStackSlotInstruction, usize)> {
        let slot_id = usize::from_le_bytes(stream[1..1 + 8].try_into().unwrap());

        let length = 1 + 8;

        Some((
            PopStackSlotInstruction {
                slot_id,
            },
            length,
        ))
    }
}
