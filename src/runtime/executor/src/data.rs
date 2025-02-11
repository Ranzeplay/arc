use log::error;
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::encodings::str_enc::StringEncoding;
use shared::models::execution::context::FunctionExecutionContext;
use shared::models::execution::data::{DataValue, DataValueType};
use shared::models::package::Package;
use std::cell::RefCell;
use std::process::exit;
use std::rc::Rc;
use shared::models::descriptors::constant::ConstantDescriptor;

pub fn get_data_from_constant_table(
    package: &Package,
    constant_id: usize,
    is_mutable: bool,
) -> DataValue {
    let constant = package.constant_table.constants.get(&constant_id).unwrap();
    let data_content = get_data_value(constant);
    DataValue {
        data_type: DataTypeEncoding {
            type_id: constant.type_id,
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

fn get_data_value(constant: &ConstantDescriptor) -> DataValueType {
    match &constant.type_id {
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
            error!("Complex type not implemented yet!");
            exit(0xffff)
        }
    }
}

pub fn get_data_from_data_slot(
    context: Rc<RefCell<FunctionExecutionContext>>,
    slot_id: usize,
) -> Rc<RefCell<DataValue>> {
    let context_ref = context.borrow();
    let slot = context_ref
        .local_data
        .iter()
        .find(|s| s.slot_id == slot_id)
        .unwrap();

    slot.value.clone()
}
