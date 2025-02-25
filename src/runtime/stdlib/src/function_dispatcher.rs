use crate::array::ArcStdArray;
use crate::console::ArcStdConsole;
use shared::models::execution::context::ExecutionContext;
use std::cell::RefCell;
use std::rc::Rc;

pub fn dispatch_stdlib_functions(function_id: usize, exec_context: Rc<RefCell<ExecutionContext>>) {
    let mut exec_context_ref = exec_context.borrow_mut();
    if function_id >= 0xa1 && function_id <= 0xaf {
        ArcStdConsole::execute_function(function_id, &mut exec_context_ref.global_stack).unwrap();
    } else if function_id >= 0xc1 && function_id <= 0xcf {
        ArcStdArray::execute_function(function_id, &mut exec_context_ref.global_stack).unwrap();
    } else {
        panic!("Unknown stdlib function");
    }
}
