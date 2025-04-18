use crate::console::ArcStdConsole;
use shared::models::execution::context::ExecutionContext;
use std::cell::RefCell;
use std::rc::Rc;
use shared::models::execution::result::FunctionExecutionResult;
use crate::ArcStdlibScope;

pub fn dispatch_stdlib_functions(function_id: usize, exec_context: Rc<RefCell<ExecutionContext>>) {
    let result = if function_id >= 0xa1 && function_id <= 0xaf {
        exec_func(ArcStdConsole{}, function_id, exec_context.clone()).unwrap()
    } else if function_id >= 0xc1 && function_id <= 0xcf {
        exec_func(ArcStdConsole{}, function_id, exec_context.clone()).unwrap()
    } else {
        panic!("Unknown stdlib function");
    };

    match result {
        FunctionExecutionResult::Success(res) => {
            if let Some(res) = res {
                exec_context.borrow_mut().global_stack.push(res);
            }
        }
        _ => panic!("Failed to execute stdlib function"),
    }
}

pub fn exec_func(scope: impl ArcStdlibScope, function_id: usize, exec_context: Rc<RefCell<ExecutionContext>>) -> Result<FunctionExecutionResult, String> {
    scope.execute_function(function_id, &mut exec_context.borrow_mut().global_stack)
}
