use shared::models::descriptors::symbol::Symbol;
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use std::cell::RefCell;
use std::rc::Rc;

pub fn prepare_and_get_function_info(function_id: usize, parent_fn_opt: Option<Rc<RefCell<FunctionExecutionContext>>>, exec_context: Rc<RefCell<ExecutionContext>>) -> Rc<RefCell<FunctionExecutionContext>> {
    let mut function_context = {
        let exec_context_ref = exec_context.borrow();
        let current_fn_symbol_opt = exec_context_ref.package.symbol_table.symbols.get(&function_id);
        let current_fn = current_fn_symbol_opt.unwrap();

        // Create the function context
        FunctionExecutionContext {
            function: match &current_fn.value {
                Symbol::Function(f) => f.clone(),
                _ => panic!("Invalid symbol type."),
            },
            local_data: Vec::with_capacity(20),
        }
    };

    put_fn_args(parent_fn_opt, exec_context, &mut function_context);

    Rc::new(RefCell::new(function_context))
}

fn put_fn_args(parent_fn_opt: Option<Rc<RefCell<FunctionExecutionContext>>>, exec_context: Rc<RefCell<ExecutionContext>>, function_context: &mut FunctionExecutionContext) {
    if let Some(_) = parent_fn_opt {
        let current_function_arg_count = function_context.function.parameter_descriptors.len();
        for i in 0..current_function_arg_count {
            let data = exec_context.borrow_mut().global_stack.pop().unwrap();
            let data = data.borrow();
            let slot = DataSlot {
                slot_id: current_function_arg_count - i - 1,
                value: Rc::new(RefCell::new(data.clone())),
            };
            function_context.local_data.push(slot);
        }

        function_context.local_data.reverse();
    } else {
        function_context.local_data.push(DataSlot {
            slot_id: 0,
            value: Rc::new(RefCell::new(DataValue {
                data_type: DataTypeEncoding {
                    type_id: 0,
                    is_array: false,
                    mutability: Mutability::Immutable,
                    memory_storage_type: MemoryStorageType::Reference,
                },
                value: DataValueType::None,
            })),
        });
    }
}
