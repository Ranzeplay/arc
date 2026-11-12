use arc_shared::models::execution::context::ExecutionContext;
use arc_stdlib::function_dispatcher::dispatch_stdlib_functions;
use std::cell::RefCell;
use std::rc::Rc;

pub fn execute_stdlib_function(function_id: usize, exec_context: Rc<RefCell<ExecutionContext>>) {
    dispatch_stdlib_functions(function_id, exec_context);
}
