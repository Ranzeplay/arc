use shared::models::execution::data::{DataValue, DataValueType};
use shared::models::execution::result::FunctionExecutionResult;
use std::cell::RefCell;
use std::rc::Rc;

pub struct ArcStdConsole {}

impl ArcStdConsole {
    pub fn print_string(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let s = args.pop().unwrap();
        let s = s.borrow();
        match &s.value {
            DataValueType::String(s) => print!("{}", s),
            _ => return Err("Expected string".to_string()),
        }

        Ok(FunctionExecutionResult::Success(None))
    }

    pub fn print_integer(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let i = args.pop().unwrap();
        let i = i.borrow();
        match i.value {
            DataValueType::Integer(i) => print!("{}", i),
            _ => return Err("Expected integer".to_string()),
        }

        Ok(FunctionExecutionResult::Success(None))
    }

    pub fn execute_function(
        function_id: usize,
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        match function_id {
            0xa1 => ArcStdConsole::print_string(args),
            0xa2 => ArcStdConsole::print_integer(args),
            _ => Err("Unknown stdlib function".to_string()),
        }
    }
}