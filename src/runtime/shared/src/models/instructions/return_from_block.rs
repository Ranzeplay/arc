use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct ReturnInstruction {
    pub with_value: bool,
}

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
