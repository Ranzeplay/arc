use std::cell::RefCell;
use std::rc::Rc;
use shared::models::execution::data::DataValue;
use shared::models::execution::result::FunctionExecutionResult;

pub mod array;
mod console;
pub mod function_dispatcher;
pub mod base;

#[macro_export]
macro_rules! dispatch_func {
    ($scope:expr, $(($id:expr, $func:path)),* $(,)?) => {
        fn execute_function(
            &self,
            function_id: usize,
            args: &mut Vec<std::rc::Rc<std::cell::RefCell<DataValue>>>,
        ) -> Result<shared::models::execution::result::FunctionExecutionResult, String> {
            match function_id {
                $(
                    $id => $func(args),
                )*
                _ => Err(format!("Unknown function in {}", $scope)),
            }
        }
    };
}

pub trait ArcStdlibScope {
    fn execute_function(&self,
        function_id: usize,
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String>;
}
