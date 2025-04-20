use arc_shared::models::descriptors::constant::{ConstantDescriptor, ConstantTable};
use arc_shared::models::encodings::str_enc::StringEncoding;
use arc_shared::models::execution::data::DataValueType;
use std::collections::HashMap;

pub fn decode_constant_table(stream: &[u8]) -> (ConstantTable, usize) {
    let mut result = HashMap::new();

    let mut pos = 0;
    let count = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;
    for _ in 0..count {
        let (constant, len) = decode_constant(&stream[pos..]);
        result.insert(constant.id, constant);
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

    let is_array = stream[pos] == 0x01;
    pos += 1;

    let len = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    let raw_value = stream[pos..pos + len].try_into().unwrap();
    let decoded_value = get_data_value(type_id, &raw_value);
    pos += len;

    (ConstantDescriptor{
        id,
        type_id,
        is_array,
        raw_value,
        decoded_value,
    }, pos)
}

fn get_data_value(type_id: usize, data: &Vec<u8>) -> DataValueType {
    match type_id {
        0 => DataValueType::None,
        1 => DataValueType::Any,
        2 => DataValueType::Integer(i64::from_le_bytes(
            data[0..8].try_into().unwrap(),
        )),
        3 => DataValueType::Decimal(f64::from_le_bytes(
            data[0..8].try_into().unwrap(),
        )),
        4 => DataValueType::Char(
            char::from_u32(u32::from_le_bytes(
                data[0..4].try_into().unwrap(),
            ))
                .unwrap(),
        ),
        5 => DataValueType::String(StringEncoding::from_u8(&data).value),
        6 => DataValueType::Bool(data[0] == 0x01),
        _ => {
            panic!("Complex type not implemented yet!");
        }
    }
}
