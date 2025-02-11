use shared::models::execution::context::ExecutionContext;
use shared::models::execution::data::DataValueType;
use std::cell::RefCell;
use std::rc::Rc;

pub fn execute_stdlib_function(function_id: usize, exec_context: Rc<RefCell<ExecutionContext>>) {
    let mut exec_context_ref = exec_context.borrow_mut();
    match function_id {
        0xa1 => {
            // Arc::Std::Console::PrintString
            let data = exec_context_ref.global_stack.pop().unwrap();

            let data = data.borrow();

            match &data.value {
                DataValueType::String(s) => {
                    print!("{}", s);
                }
                _ => {
                    panic!("Invalid data type")
                }
            }
        }
        0xa2 => {
            // Arc::Std::Console::PrintInteger
            let data = exec_context_ref.global_stack.pop().unwrap();

            let data = data.borrow();

            match &data.value {
                DataValueType::Integer(i) => {
                    print!("{}", i);
                }
                _ => {
                    panic!("Invalid data type")
                }
            }
        }
        _ => {
            panic!("Unknown stdlib function")
        }
    }
}
