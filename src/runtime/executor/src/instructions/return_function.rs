use shared::models::execution::context::ExecutionContext;
use shared::models::execution::result::FunctionExecutionResult;
use std::cell::RefCell;
use std::rc::Rc;

pub fn wrap_return_value_if_needed(
    exec_context: Rc<RefCell<ExecutionContext>>,
    with_value: bool
) -> FunctionExecutionResult {
    if with_value {
        let data = exec_context
            .borrow_mut()
            .global_stack
            .pop()
            .unwrap();
        FunctionExecutionResult::Success(Some(data.to_owned()))
    } else {
        FunctionExecutionResult::Success(None)
    }
}
