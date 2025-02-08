use shared::models::execution::data::{DataValue, DataValueType};

macro_rules! arithmetic_operations {
    ($name:ident, $op:tt, $op_name:expr) => {
        pub fn $name(a: &DataValue, b: &DataValue) -> DataValue {
            assert_eq!(a.data_type.type_id, b.data_type.type_id, concat!("Cannot ", $op_name, " values of different types"));

            match (&a.value, &b.value) {
                (DataValueType::Integer(ad), DataValueType::Integer(bd)) => DataValue {
                    data_type: a.data_type.clone(),
                    value: DataValueType::Integer(ad $op bd),
                },
                (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => DataValue {
                    data_type: a.data_type.clone(),
                    value: DataValueType::Decimal(ad $op bd),
                },
                _ => panic!(concat!("Cannot ", $op_name, " values of different types")),
            }
        }
    };
}

arithmetic_operations!(math_add_values, +, "add");
arithmetic_operations!(math_subtract_values, -, "subtract");
arithmetic_operations!(math_multiply_values, *, "multiply");
arithmetic_operations!(math_divide_values, /, "divide");

macro_rules! compare_operations {
    ($name:ident, $op:tt) => {
        pub fn $name(a: &DataValue, b: &DataValue) -> bool {
            if a.data_type.type_id != b.data_type.type_id {
                panic!(concat!("Cannot compare values of different types"));
            }

            match (&a.value, &b.value) {
                (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad $op bd,
                (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad $op bd,
                _ => panic!(concat!("Cannot compare values of different types")),
            }
        }
    };
}

compare_operations!(math_compare_less, <);
compare_operations!(math_compare_greater, >);
compare_operations!(math_compare_less_or_equal, <=);
compare_operations!(math_compare_greater_or_equal, >=);

pub fn math_compare_equal(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad == bd,
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad == bd,
        (DataValueType::String(ad), DataValueType::String(bd)) => ad == bd,
        (DataValueType::Bool(ad), DataValueType::Bool(bd)) => ad == bd,
        (DataValueType::None, DataValueType::None) => true,
        _ => panic!("Cannot compare values of different types"),
    }
}

pub fn math_compare_not_equal(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad != bd,
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad != bd,
        (DataValueType::String(ad), DataValueType::String(bd)) => ad != bd,
        (DataValueType::Bool(ad), DataValueType::Bool(bd)) => ad != bd,
        (DataValueType::None, DataValueType::None) => true,
        _ => panic!("Cannot compare values of different types"),
    }
}
