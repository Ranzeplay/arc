use shared::models::descriptors::constant::{ConstantDescriptor, ConstantTable};

pub fn decode_constant_table(stream: &[u8]) -> (ConstantTable, usize) {
    let mut result = vec![];

    let mut pos = 0;
    let count = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;
    for _ in 0..count {
        let (constant, len) = decode_constant(&stream[pos..]);
        result.push(constant);
        pos += len;
    }

    (ConstantTable { constants: result }, pos)
}

fn decode_constant(stream: &[u8]) -> (ConstantDescriptor, usize) {
    let mut pos = 0;
    let id = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;

    let type_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    let len = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    let raw_value = stream[pos..pos + len].try_into().unwrap();

    (ConstantDescriptor{
        id,
        type_id,
        raw_value,
    }, pos)
}
