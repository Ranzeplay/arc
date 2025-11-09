use arc_shared::base_type_id::STRING_TYPE_ID;
use arc_shared::models::descriptors::symbol::Symbol;
use arc_shared::models::encodings::data_type_enc::{DataTypeEncoding, Mutability};
use arc_shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use arc_shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use std::cell::RefCell;
use std::ops::Deref;
use std::rc::Rc;
use crate::instructions::data_declaration::construct_data;

pub fn prepare_and_get_function_info(
    function_id: usize,
    parent_fn_opt: Option<Rc<RefCell<FunctionExecutionContext>>>,
    exec_context: Rc<RefCell<ExecutionContext>>,
    is_constructor_of_type: Option<usize>,
    param_count: usize,
) -> Rc<RefCell<FunctionExecutionContext>> {
    let mut temp_param_store = vec![];
    if function_id == 0 {
        let mut exec_context_ref = exec_context.borrow_mut();
        for _ in 0..param_count {
            let data = exec_context_ref.global_stack.pop().unwrap();
            temp_param_store.push(data);
        }
    }

    let mut function_context = {
        let mut exec_context_ref = exec_context.borrow_mut();
        let mut fn_id = function_id;
        if function_id == 0 {
            let st_symbol = exec_context_ref.global_stack.pop().unwrap();
            let st_symbol = st_symbol.borrow();
            fn_id = match &st_symbol.value {
                DataValueType::Symbol(s) => {
                    s.id
                },
                _ => panic!("Invalid data type for function symbol")
            }
        }
        let current_fn_symbol_opt = exec_context_ref
            .package
            .symbol_table
            .symbols
            .get(&fn_id);
        let current_fn = current_fn_symbol_opt.unwrap();

        // Create the function context
        match &current_fn.value {
            Symbol::Function(f) => FunctionExecutionContext {
                id: fn_id,
                function: f.clone(),
                local_data: Vec::with_capacity(f.data_count),
            },
            _ => panic!("Invalid symbol type."),
        }
    };

    if function_id == 0 {
        let mut exec_context_ref = exec_context.borrow_mut();
        // Push back into global stack
        for data in temp_param_store.into_iter().rev() {
            exec_context_ref.global_stack.push(data);
        }
    }

    if let Some(_) = parent_fn_opt {
        put_fn_args(Rc::clone(&exec_context), &mut function_context, is_constructor_of_type);
    } else {
        put_cmdline_fn_args(Rc::clone(&exec_context), &mut function_context);
    }

    if function_id == 0 {
        function_context.id = {
            let mut exec_context_ref = exec_context.borrow_mut();
            let symbol_data = exec_context_ref.global_stack.pop().unwrap();
            let symbol_data = symbol_data.borrow();

            match &symbol_data.value {
                DataValueType::Symbol(s) => s.id,
                _ => panic!("Invalid data type for function symbol")
            }
        };
    }

    Rc::new(RefCell::new(function_context))
}

fn put_fn_args(
    exec_context: Rc<RefCell<ExecutionContext>>,
    function_context: &mut FunctionExecutionContext,
    is_constructor_of_type: Option<usize>,
) {

    let current_function_arg_count = function_context.function.parameter_descriptors.len();
    let begin_index = if is_constructor_of_type.is_none() { 0 } else { 1 };
    for i in begin_index..current_function_arg_count {
        let data = exec_context.borrow_mut().global_stack.pop().unwrap();
        let slot_id = current_function_arg_count - i - 1;
        let target_arg = &function_context.function.parameter_descriptors[slot_id];

        let slot = DataSlot {
            slot_id,
            value: Rc::new(RefCell::new(DataValue {
                data_type: target_arg.deref().clone(),
                value: data.borrow().value.clone(),
            }))
        };
        function_context
            .local_data
            .push(Rc::new(RefCell::new(slot)));
    }

    if let Some(type_id) = is_constructor_of_type {
        let exec_context_ref = exec_context.borrow();
        let obj = construct_data(type_id, &exec_context_ref.package);
        function_context.local_data.push(Rc::new(RefCell::new(DataSlot {
            slot_id: 0,
            value: obj,
        })));
    }

    function_context.local_data.reverse();
}

fn put_cmdline_fn_args(
    exec_context: Rc<RefCell<ExecutionContext>>,
    function_context: &mut FunctionExecutionContext,
) {
    function_context
        .local_data
        .push(Rc::new(RefCell::new(DataSlot {
            slot_id: 0,
            value: Rc::new(RefCell::new(DataValue {
                data_type: DataTypeEncoding {
                    type_id: *STRING_TYPE_ID,
                    dimension: 1,
                    mutability: Mutability::Mutable,
                },
                value: DataValueType::Array(
                    exec_context
                        .borrow()
                        .launch_args
                        .iter()
                        .map(|arg| Rc::new(RefCell::new(DataValue::from(arg.clone()))))
                        .collect(),
                ),
            })),
        })));
}
