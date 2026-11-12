use crate::models::execution::context::ExecutionContext;
use crate::models::execution::result::FunctionExecutionResult;
use std::cell::RefCell;
use std::rc::Rc;

pub trait ScopeFunctionDispatcher {
    fn dispatch_scope_functions(
        &self,
        function_id: usize,
        exec_context: Rc<RefCell<ExecutionContext>>,
    ) -> Result<FunctionExecutionResult, String>;
}
