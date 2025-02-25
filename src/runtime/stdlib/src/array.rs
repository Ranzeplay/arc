use std::cell::RefCell;
use std::rc::Rc;
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::data::{DataValue, DataValueType};
use shared::models::execution::result::FunctionExecutionResult;

pub struct ArcStdArray {}

impl ArcStdArray {
    pub fn create_int_array(
        _args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let data_type = DataTypeEncoding {
            type_id: 2,
            dimension: 1,
            mutability: Mutability::Mutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::Array(vec![]);

        let data = DataValue {
            data_type,
            value,
        };

        Ok(FunctionExecutionResult::Success(Some(Rc::new(RefCell::new(data)))))
    }

    pub fn push_int_array(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let value = args.pop().unwrap();
        let array = args.pop().unwrap();

        let mut array = array.borrow_mut();

        match &mut array.value {
            DataValueType::Array(array) => {
                match &value.borrow().value {
                    DataValueType::Integer(_) => array.push(Rc::clone(&value)),
                    _ => return Err("Expected integer".to_string()),
                }
            }
            _ => return Err("Expected array".to_string()),
        }

        Ok(FunctionExecutionResult::Success(None))
    }

    pub fn remove_element_from_int_array(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let index = args.pop().unwrap();
        let array = args.pop().unwrap();

        let mut array = array.borrow_mut();

        match &mut array.value {
            DataValueType::Array(array) => {
                match &index.borrow().value {
                    DataValueType::Integer(i) => {
                        let i = *i as usize;
                        array.remove(i);
                    }
                    _ => return Err("Expected integer".to_string()),
                }
            }
            _ => return Err("Expected array".to_string()),
        }

        Ok(FunctionExecutionResult::Success(None))
    }

    pub fn get_int_array_size(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let array = args.pop().unwrap();

        let array = array.borrow();

        match &array.value {
            DataValueType::Array(array) => {
                let size = array.len();
                let data_type = DataTypeEncoding {
                    type_id: 1,
                    dimension: 0,
                    mutability: Mutability::Immutable,
                    memory_storage_type: MemoryStorageType::Value,
                };

                let value = DataValueType::Integer(size as i64);

                let data = DataValue {
                    data_type,
                    value,
                };

                Ok(FunctionExecutionResult::Success(Some(Rc::new(RefCell::new(data)))))
            }
            _ => Err("Expected array".to_string()),
        }
    }

    pub fn execute_function(
        function_id: usize,
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        match function_id {
            0xc1 => ArcStdArray::create_int_array(args),
            0xc2 => ArcStdArray::push_int_array(args),
            0xc3 => ArcStdArray::remove_element_from_int_array(args),
            0xc4 => ArcStdArray::get_int_array_size(args),
            _ => Err("Unknown stdlib array function".to_string()),
        }
    }
}