use crate::execute_function;
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::result::FunctionExecutionResult;
use std::cell::RefCell;
use std::rc::Rc;

pub fn execute_function_call(
    exec_context: Rc<RefCell<ExecutionContext>>,
    fn_context: Rc<RefCell<FunctionExecutionContext>>,
    function_id: usize
) {
    let call_result = execute_function(function_id, Some(Rc::clone(&fn_context)), Rc::clone(&exec_context));
    match call_result {
        FunctionExecutionResult::Success(data_opt) => {
            if let Some(data) = data_opt {
                exec_context.borrow_mut().global_stack.push(data);
            }
        }
        FunctionExecutionResult::Failure(x) => {
            panic!("Function execution failed: {}", x.fault_message);
        }
        FunctionExecutionResult::Invalid => {
            panic!("Invalid function execution result.");
        }
    }
}
