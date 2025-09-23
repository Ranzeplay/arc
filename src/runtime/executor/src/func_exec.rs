use arc_shared::base_type_id::STRING_TYPE_ID;
use arc_shared::models::descriptors::symbol::Symbol;
use arc_shared::models::encodings::data_type_enc::{DataTypeEncoding, Mutability};
use arc_shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use arc_shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use std::cell::RefCell;
use std::ops::Deref;
use std::rc::Rc;

pub fn prepare_and_get_function_info(
    function_id: usize,
    parent_fn_opt: Option<Rc<RefCell<FunctionExecutionContext>>>,
    exec_context: Rc<RefCell<ExecutionContext>>,
) -> Rc<RefCell<FunctionExecutionContext>> {
    let mut function_context = {
        let exec_context_ref = exec_context.borrow();
        let current_fn_symbol_opt = exec_context_ref
            .package
            .symbol_table
            .symbols
            .get(&function_id);
        let current_fn = current_fn_symbol_opt.unwrap();

        // Create the function context
        match &current_fn.value {
            Symbol::Function(f) => FunctionExecutionContext {
                function: f.clone(),
                local_data: Vec::with_capacity(f.data_count),
            },
            _ => panic!("Invalid symbol type."),
        }
    };

    put_fn_args(parent_fn_opt, exec_context, &mut function_context);

    Rc::new(RefCell::new(function_context))
}

fn put_fn_args(
    parent_fn_opt: Option<Rc<RefCell<FunctionExecutionContext>>>,
    exec_context: Rc<RefCell<ExecutionContext>>,
    function_context: &mut FunctionExecutionContext,
) {
    if let Some(_) = parent_fn_opt {
        let current_function_arg_count = function_context.function.parameter_descriptors.len();
        for i in 0..current_function_arg_count {
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

        function_context.local_data.reverse();
    } else {
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
}
