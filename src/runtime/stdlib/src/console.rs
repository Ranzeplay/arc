use shared::models::execution::data::{DataValue, DataValueType};
use shared::models::execution::result::FunctionExecutionResult;
use std::cell::RefCell;
use std::rc::Rc;
use bindings::{arc_function_id, arc_scope_dispatcher};
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use crate::base::{INTEGER_TYPE_ID, STRING_TYPE_ID};

pub struct ArcStdConsole {}

#[arc_scope_dispatcher("Arc::Std::Console")]
impl ArcStdConsole {
    #[arc_function_id(0xa1)]
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

    #[arc_function_id(0xa2)]
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

    #[arc_function_id(0xa3)]
    pub fn read_string(_args: &mut Vec<Rc<RefCell<DataValue>>>) -> Result<FunctionExecutionResult, String> {
        let mut input = String::new();
        std::io::stdin().read_line(&mut input).unwrap();

        let data_type = DataTypeEncoding {
            type_id: *STRING_TYPE_ID,
            dimension: 0,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::String(input);

        Ok(FunctionExecutionResult::Success(Some(Rc::new(RefCell::new(DataValue{ data_type, value })))))
    }

    #[arc_function_id(0xa4)]
    pub fn read_integer(_args: &mut Vec<Rc<RefCell<DataValue>>>) -> Result<FunctionExecutionResult, String> {
        let mut input = String::new();
        std::io::stdin().read_line(&mut input).unwrap();
        let input = input.trim().parse::<i64>().unwrap();

        let data_type = DataTypeEncoding {
            type_id: *INTEGER_TYPE_ID,
            dimension: 0,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::Integer(input);

        Ok(FunctionExecutionResult::Success(Some(Rc::new(RefCell::new(DataValue{ data_type, value })))))
    }
}
