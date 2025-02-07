use shared::models::execution::data::{DataValue, DataValueType};

pub fn math_add_values(a: &DataValue, b: &DataValue) -> DataValue {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot add values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Integer(ad + bd),
            }
        }
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Decimal(ad + bd),
            }
        },
        (DataValueType::String(ad), DataValueType::String(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::String(format!("{}{}", ad, bd)),
            }
        }
        _ => panic!("Cannot add values of different types"),
    }
}

pub fn math_subtract_values(a: &DataValue, b: &DataValue) -> DataValue {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot subtract values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Integer(ad - bd),
            }
        }
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Decimal(ad - bd),
            }
        },
        _ => panic!("Cannot subtract values of different types"),
    }
}

pub fn math_multiply_values(a: &DataValue, b: &DataValue) -> DataValue {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot multiply values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Integer(ad * bd),
            }
        }
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Decimal(ad * bd),
            }
        },
        _ => panic!("Cannot multiply values of different types"),
    }
}

pub fn math_divide_values(a: &DataValue, b: &DataValue) -> DataValue {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot divide values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Integer(ad / bd),
            }
        }
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => {
            DataValue {
                data_type: a.data_type.clone(),
                value: DataValueType::Decimal(ad / bd),
            }
        },
        _ => panic!("Cannot divide values of different types"),
    }
}

pub fn math_compare_less(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad < bd,
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad < bd,
        _ => panic!("Cannot compare values of different types"),
    }
}

pub fn math_compare_greater(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad > bd,
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad > bd,
        _ => panic!("Cannot compare values of different types"),
    }
}

pub fn math_compare_equal(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad == bd,
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad == bd,
        (DataValueType::String(ad), DataValueType::String(bd)) => ad == bd,
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
        _ => panic!("Cannot compare values of different types"),
    }
}

pub fn math_compare_less_or_equal(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad <= bd,
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad <= bd,
        _ => panic!("Cannot compare values of different types"),
    }
}

pub fn math_compare_greater_or_equal(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Integer(ad), DataValueType::Integer(bd)) => ad >= bd,
        (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => ad >= bd,
        _ => panic!("Cannot compare values of different types"),
    }
}
