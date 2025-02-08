use std::cell::RefCell;
use std::rc::Rc;
use shared::models::execution::context::ExecutionContext;
use shared::models::execution::data::DataValueType;

pub fn execute_stdlib_function(function_id: usize, context: &Rc<RefCell<ExecutionContext>>) {
    let function_context = context.borrow().stack_frames.back().unwrap().clone();
    match function_id {
        0xa1 => {
            // Arc::Std::Console::PrintString
            let mut fn_context_ref = function_context.borrow_mut();
            let data = fn_context_ref.local_stack.pop().unwrap();

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
            let mut fn_context_ref = function_context.borrow_mut();
            let data = fn_context_ref.local_stack.pop().unwrap();

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
