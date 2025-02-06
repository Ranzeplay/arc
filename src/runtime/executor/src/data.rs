use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::context::ExecutionContext;
use shared::models::execution::data::{DataValue, DataValueType};
use std::cell::RefCell;
use std::rc::Rc;
use shared::models::encodings::str_enc::StringEncoding;

pub fn get_data_from_constant_table(
    context: Rc<RefCell<ExecutionContext>>,
    constant_id: usize,
    is_mutable: bool,
) -> DataValue {
    let context_ref = context.borrow();
    let constant = context_ref
        .package
        .constant_table
        .constants
        .iter()
        .find(|c| c.id == constant_id)
        .unwrap();

    let data_type_symbol = context_ref
        .package
        .symbol_table
        .symbols
        .iter()
        .find(|s| s.id == constant.type_id)
        .unwrap();

    let data_content = match data_type_symbol.id {
        0 => DataValueType::None,
        1 => DataValueType::Any,
        2 => DataValueType::Integer(i64::from_le_bytes(
            constant.raw_value[0..8].try_into().unwrap(),
        )),
        3 => DataValueType::Decimal(f64::from_le_bytes(
            constant.raw_value[0..8].try_into().unwrap(),
        )),
        4 => DataValueType::Char(
            char::from_u32(u32::from_le_bytes(
                constant.raw_value[0..4].try_into().unwrap(),
            ))
            .unwrap(),
        ),
        5 => DataValueType::String(StringEncoding::from_u8(&constant.raw_value).value),
        6 => DataValueType::Bool(constant.raw_value[0] == 0x01),
        _ => {
            panic!("Complex pe not implemented yet!")
        }
    };

    DataValue {
        data_type: DataTypeEncoding {
            type_id: data_type_symbol.id,
            is_array: constant.is_array,
            mutability: if is_mutable {
                Mutability::Mutable
            } else {
                Mutability::Immutable
            },
            memory_storage_type: MemoryStorageType::Value,
        },
        value: data_content,
    }
}
