use std::cell::RefCell;
use std::rc::Rc;
use shared::models::execution::context::FunctionExecutionContext;
use shared::models::execution::result::FunctionExecutionResult;

pub fn wrap_return_value_if_needed(
    function_context: &Rc<RefCell<FunctionExecutionContext>>,
    with_value: bool
) -> FunctionExecutionResult {
    if with_value {
        let data = function_context.borrow_mut().local_stack.pop().unwrap();
        FunctionExecutionResult::Success(Some(data))
    } else {
        FunctionExecutionResult::Success(None)
    }
}
