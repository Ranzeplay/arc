use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct PopToSlotInstruction {
    pub slot_id: usize,
}

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
