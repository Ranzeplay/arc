use arc_shared::models::execution::data::{DataValue, DataValueType};

macro_rules! arithmetic_operations {
    ($name:ident, $op:tt, $op_name:expr) => {
        pub fn $name(a: &DataValue, b: &DataValue) -> DataValue {
            match (&a.value, &b.value) {
                (DataValueType::Integer(ad), DataValueType::Integer(bd)) => DataValue {
                    data_type: a.data_type.clone(),
                    value: DataValueType::Integer(ad $op bd),
                },
                (DataValueType::Decimal(ad), DataValueType::Decimal(bd)) => DataValue {
                    data_type: a.data_type.clone(),
                    value: DataValueType::Decimal(ad $op bd),
                },
                _ => {
                    panic!("Cannot {} values of different types: 0x{:08X} 0x{:08X}",
                        $op_name,
                        a.data_type.type_id,
                        b.data_type.type_id
                    )
                },
            }
        }
    };
}

arithmetic_operations!(math_add_values, +, "add");
arithmetic_operations!(math_subtract_values, -, "subtract");
arithmetic_operations!(math_multiply_values, *, "multiply");
arithmetic_operations!(math_divide_values, /, "divide");
arithmetic_operations!(math_modulo_values, %, "modulo");

macro_rules! compare_operations {
    ($name:ident, $op:tt) => {
        #[inline]
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

pub fn math_logical_and(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Bool(ad), DataValueType::Bool(bd)) => *ad && *bd,
        _ => panic!("Cannot compare values of different types"),
    }
}

pub fn math_logical_or(a: &DataValue, b: &DataValue) -> bool {
    if a.data_type.type_id != b.data_type.type_id {
        panic!("Cannot compare values of different types");
    }

    match (&a.value, &b.value) {
        (DataValueType::Bool(ad), DataValueType::Bool(bd)) => *ad || *bd,
        _ => panic!("Cannot compare values of different types"),
    }
}

pub fn math_logical_not(a: &DataValue) -> bool {
    match &a.value {
        DataValueType::Bool(ad) => !*ad,
        _ => panic!("Cannot compare values of different types"),
    }
}

#[macro_export]
macro_rules! arithmetic_operation_instruction {
    ($exec_context:expr, $op_func:expr) => {
        let mut exec_context_ref = $exec_context.borrow_mut();
        let a = exec_context_ref.global_stack.pop().unwrap();
        let b = exec_context_ref.global_stack.pop().unwrap();

        let result = $op_func(&b.borrow(), &a.borrow());
        exec_context_ref
            .global_stack
            .push(Rc::new(RefCell::new(result)));
    };
}

#[macro_export]
macro_rules! comparison_operation_instruction {
    ($fn_context:expr, $compare_func:expr) => {
        let mut exec_context_ref = $fn_context.borrow_mut();
        let a = exec_context_ref.global_stack.pop().unwrap();
        let b = exec_context_ref.global_stack.pop().unwrap();

        let result = $compare_func(&b.borrow(), &a.borrow());
        push_bool_to_stack!(exec_context_ref.global_stack, result);
    };
}
