use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::context::FunctionExecutionContext;
use shared::models::execution::data::DataValue;
use shared::models::package::Package;
use std::cell::RefCell;
use std::rc::Rc;

pub fn get_data_from_constant_table(
    package: &Package,
    constant_id: usize,
    is_mutable: bool,
) -> DataValue {
    let constant = package.constant_table.constants.get(&constant_id).unwrap();
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
        value: constant.decoded_value.clone(),
    }
}

pub fn get_data_from_data_slot(
    context: Rc<RefCell<FunctionExecutionContext>>,
    slot_id: usize,
) -> Rc<RefCell<DataValue>> {
    let context_ref = context.borrow();
    let slot = context_ref
        .local_data
        .get(slot_id)
        .unwrap();

    slot.value.clone()
}
