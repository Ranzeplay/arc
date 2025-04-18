use arc_bindings::{arc_function_id, arc_scope_dispatcher};
use arc_shared::models::execution::data::DataValue;
use arc_shared::models::execution::result::FunctionExecutionResult;
use arc_shared::receive_func_args;
use std::cell::RefCell;
use std::rc::Rc;

pub struct ArcStdConsole {}

#[arc_scope_dispatcher("Arc::Std::Console")]
impl ArcStdConsole {
    #[arc_function_id(0xa1)]
    pub fn print_string(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let s = receive_func_args!(args, String);

        print!("{}", s);

        Ok(FunctionExecutionResult::Success(None))
    }

    #[arc_function_id(0xa2)]
    pub fn print_integer(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let i = receive_func_args!(args, i64);
        print!("{}", i);

        Ok(FunctionExecutionResult::Success(None))
    }

    #[arc_function_id(0xa3)]
    pub fn read_string(_args: &mut Vec<Rc<RefCell<DataValue>>>) -> Result<FunctionExecutionResult, String> {
        let mut input = String::new();
        std::io::stdin().read_line(&mut input).unwrap();

        Ok(FunctionExecutionResult::Success(Some(Rc::new(RefCell::new(DataValue::from(input))))))
    }

    #[arc_function_id(0xa4)]
    pub fn read_integer(_args: &mut Vec<Rc<RefCell<DataValue>>>) -> Result<FunctionExecutionResult, String> {
        let mut input = String::new();
        std::io::stdin().read_line(&mut input).unwrap();
        let input = input.trim().parse::<i64>().unwrap();

        Ok(FunctionExecutionResult::Success(Some(Rc::new(RefCell::new(DataValue::from(input))))))
    }
}
