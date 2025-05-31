use std::fmt::Debug;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Copy, Clone)]
pub struct NewObjectInstruction {
    pub type_id: usize
}

impl Debug for NewObjectInstruction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "0x{:016X}", self.type_id)
    }
}

impl DecodableInstruction<NewObjectInstruction> for NewObjectInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(NewObjectInstruction, usize)> {
        let type_id = usize::from_le_bytes(stream[1..9].try_into().unwrap());
        let length = 9;

        Some((
            NewObjectInstruction {
                type_id,
            },
            length,
        ))
    }
}
