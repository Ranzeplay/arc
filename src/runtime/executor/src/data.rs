use arc_shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use arc_shared::models::execution::data::DataValue;
use arc_shared::models::package::Package;

pub fn get_data_from_constant_table(
    package: &Package,
    constant_id: usize,
    is_mutable: bool,
) -> DataValue {
    let constant = package.constant_table.constants.get(&constant_id).unwrap();
    DataValue {
        data_type: DataTypeEncoding {
            type_id: constant.type_id,
            dimension: constant.is_array as u32,
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
