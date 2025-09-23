use pad::PadStr;
use std::fmt::{Debug, Formatter};
pub use crate::models::instruction_type::InstructionType;

pub struct Instruction {
    pub instruction_type: InstructionType,
    pub offset: usize,
    pub raw: Vec<u8>,
}

impl Debug for Instruction {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        let description = format!("{:?}", self.instruction_type);

        write!(
            f,
            "0x{:08X} {:4} {:<8} {} {:02X?}",
            self.offset,
            ' ',
            self.instruction_type.as_ref(),
            description.pad_to_width(48),
            self.raw
        )
    }
}
