use shared::models::execution::context::ExecutionContext;
use std::cell::RefCell;
use std::rc::Rc;
use arc_stdlib::function_dispatcher::dispatch_stdlib_functions;

pub fn execute_stdlib_function(function_id: usize, exec_context: Rc<RefCell<ExecutionContext>>) {
    dispatch_stdlib_functions(function_id, exec_context);
}
