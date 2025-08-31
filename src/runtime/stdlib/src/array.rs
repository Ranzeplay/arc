use arc_bindings::{arc_function_id, arc_scope_dispatcher};
use arc_shared::base_type_id::{INTEGER_TYPE_ID, NONE_TYPE_ID};
use arc_shared::models::encodings::data_type_enc::{
    DataTypeEncoding, MemoryStorageType, Mutability,
};
use arc_shared::models::execution::data::{DataValue, DataValueType};
use arc_shared::models::execution::result::FunctionExecutionResult;
use std::cell::RefCell;
use std::rc::Rc;

pub struct ArcStdArray {}

#[arc_scope_dispatcher("Arc::Std::Array")]
impl ArcStdArray {
    #[arc_function_id(0xc1)]
    pub fn create_int_array(
        _args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let data_type = DataTypeEncoding {
            type_id: *INTEGER_TYPE_ID,
            dimension: 1,
            mutability: Mutability::Mutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::Array(vec![]);

        let data = DataValue { data_type, value };

        Ok(FunctionExecutionResult::Success(Some(Rc::new(
            RefCell::new(data),
        ))))
    }

    #[arc_function_id(0xc2)]
    pub fn push_int_array(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let value = args.pop().unwrap();
        let array = args.pop().unwrap();

        let mut array = array.borrow_mut();
        let memory_storage_type = array.data_type.memory_storage_type.clone();

        match &mut array.value {
            DataValueType::Array(arr) => match &value.borrow().value {
                DataValueType::Integer(_) => {
                    let integer_value = {
                        match memory_storage_type {
                            MemoryStorageType::Reference => Rc::clone(&value),
                            MemoryStorageType::Value => {
                                Rc::new(RefCell::new(value.borrow().clone()))
                            }
                        }
                    };

                    arr.push(integer_value);
                }
                _ => return Err("Expected integer".to_string()),
            },
            _ => return Err("Expected array".to_string()),
        };

        Ok(FunctionExecutionResult::Success(None))
    }

    #[arc_function_id(0xc3)]
    pub fn remove_element_from_int_array(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let index = args.pop().unwrap();
        let array = args.pop().unwrap();

        let mut array = array.borrow_mut();

        match &mut array.value {
            DataValueType::Array(array) => match &index.borrow().value {
                DataValueType::Integer(i) => {
                    let i = *i as usize;
                    array.remove(i);
                }
                _ => return Err("Expected integer".to_string()),
            },
            _ => return Err("Expected array".to_string()),
        }

        Ok(FunctionExecutionResult::Success(None))
    }

    #[arc_function_id(0xc4)]
    pub fn get_int_array_size(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let array = args.pop().unwrap();

        let array = array.borrow();

        match &array.value {
            DataValueType::Array(array) => {
                let size = array.len();

                Ok(FunctionExecutionResult::Success(Some(Rc::new(
                    RefCell::new(DataValue::from(size as i64)),
                ))))
            }
            _ => Err("Expected array".to_string()),
        }
    }

    #[arc_function_id(0xc5)]
    pub fn resize_int_array(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let capacity = args.pop().unwrap();
        let array = args.pop().unwrap();

        let mut array = array.borrow_mut();

        match &mut array.value {
            DataValueType::Array(array) => match &capacity.borrow().value {
                DataValueType::Integer(i) => {
                    let i = *i as usize;
                    array.resize(
                        i,
                        Rc::new(RefCell::new(DataValue {
                            data_type: DataTypeEncoding {
                                type_id: *NONE_TYPE_ID,
                                dimension: 0,
                                mutability: Mutability::Mutable,
                                memory_storage_type: MemoryStorageType::Reference,
                            },
                            value: DataValueType::None,
                        })),
                    );
                }
                _ => return Err("Expected integer".to_string()),
            },
            _ => return Err("Expected array".to_string()),
        }

        Ok(FunctionExecutionResult::Success(None))
    }

    #[arc_function_id(0xc6)]
    pub fn create_int_array_p(
        args: &mut Vec<Rc<RefCell<DataValue>>>,
    ) -> Result<FunctionExecutionResult, String> {
        let size = args.pop().unwrap();

        let size = size.borrow();

        match &size.value {
            DataValueType::Integer(i) => {
                let i = *i as usize;

                let data_type = DataTypeEncoding {
                    type_id: *INTEGER_TYPE_ID,
                    dimension: 1,
                    mutability: Mutability::Mutable,
                    memory_storage_type: MemoryStorageType::Value,
                };

                let value = DataValueType::Array(Vec::with_capacity(i));

                let data = DataValue { data_type, value };

                Ok(FunctionExecutionResult::Success(Some(Rc::new(
                    RefCell::new(data),
                ))))
            }
            _ => Err("Expected integer".to_string()),
        }
    }
}
