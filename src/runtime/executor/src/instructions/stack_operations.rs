use crate::data;
use arc_shared::models::encodings::data_type_enc::MemoryStorageType;
use arc_shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use arc_shared::models::execution::data::{DataValue, DataValueType};
use arc_shared::models::instructions::pop_to_slot::PopToSlotInstruction;
use arc_shared::models::instructions::stack_data_operation::{
    DataSourceType, StackOperationInstruction,
};
use std::cell::RefCell;
use std::rc::Rc;

pub fn load_stack(
    exec_context: &Rc<RefCell<ExecutionContext>>,
    function_context: Rc<RefCell<FunctionExecutionContext>>,
    lsi: &StackOperationInstruction,
) {
    let package = {
        let exec_context_ref = exec_context.borrow();
        Rc::new(exec_context_ref.package.clone())
    };
    let stack = &mut exec_context.borrow_mut().global_stack;

    let data = match lsi.source {
        DataSourceType::ConstantTable => {
            let value = data::get_data_from_constant_table(&package, lsi.location_id, true);

            Rc::new(RefCell::new(value))
        }
        DataSourceType::DataSlot => {
            let context_ref = function_context.borrow();
            let slot = context_ref.local_data.get(lsi.location_id).unwrap();

            let r = Rc::clone(&slot.borrow().value);

            r
        }
        DataSourceType::StackTop => {
            if lsi.overwrite {
                stack.pop().unwrap().to_owned()
            } else {
                stack.last().unwrap().to_owned()
            }
        }
        DataSourceType::Field => {
            let stack_top = if lsi.overwrite {
                stack.pop().unwrap().to_owned()
            } else {
                stack.last().unwrap().to_owned()
            };
            let field_id = lsi.location_id;

            let stack_top_ref = stack_top.borrow();
            let complex_data = match &stack_top_ref.value {
                DataValueType::Complex(d) => d,
                _ => panic!("Invalid data type"),
            };

            Rc::clone(&complex_data.values[&field_id])
        }
        DataSourceType::ArrayElement => {
            let (stack_top_index_value, stack_top_array_value) = {
                if lsi.overwrite {
                    (
                        stack.pop().unwrap().to_owned(),
                        stack.pop().unwrap().to_owned(),
                    )
                } else {
                    (
                        stack.last().unwrap().to_owned(),
                        stack.last().unwrap().to_owned(),
                    )
                }
            };

            let index = match &stack_top_index_value.borrow().value {
                DataValueType::Integer(i) => *i as usize,
                _ => panic!("Invalid data type"),
            };
            let stack_top_array_value = stack_top_array_value.borrow();
            let array_element = match &stack_top_array_value.value {
                DataValueType::Array(a) => Rc::clone(&a[index]),
                DataValueType::String(s) => Rc::new(RefCell::new(DataValue::from(s.chars().nth(index).unwrap()))),
                _ => panic!("Invalid data type"),
            };

            array_element
        }
    };

    let data = match &lsi.storage_type {
        MemoryStorageType::Reference => Rc::clone(&data),
        MemoryStorageType::Value => Rc::new(RefCell::new(data.borrow().to_owned().clone())),
    };

    stack.push(data);
}

pub fn save_stack(
    exec_context: &Rc<RefCell<ExecutionContext>>,
    function_context: Rc<RefCell<FunctionExecutionContext>>,
    ssi: &StackOperationInstruction,
) {
    let stack_top_data = {
        let mut exec_context_ref = exec_context.borrow_mut();
        exec_context_ref.global_stack.pop().unwrap()
    };

    match ssi.source {
        DataSourceType::ConstantTable => {
            panic!("Cannot overwrite the constant table");
        }
        DataSourceType::DataSlot => {
            let context_ref = function_context.borrow();
            let slot = context_ref.local_data.get(ssi.location_id).unwrap();

            let mut slot_ref = slot.borrow_mut();

            match ssi.storage_type {
                MemoryStorageType::Reference => {
                    slot_ref.value = stack_top_data;
                }
                MemoryStorageType::Value => {
                    slot_ref.value = Rc::new(RefCell::new(stack_top_data.borrow().clone()));
                }
            }
        }
        DataSourceType::Field => {}
        DataSourceType::ArrayElement => {}
        DataSourceType::StackTop => panic!("Cannot overwrite the stack top"),
    }
}

pub fn pop_to_slot(
    exec_context: &Rc<RefCell<ExecutionContext>>,
    function_context: &Rc<RefCell<FunctionExecutionContext>>,
    pops: &PopToSlotInstruction,
) {
    let fn_context_ref = function_context.borrow();
    let mut exec_context_ref = exec_context.borrow_mut();
    let data = exec_context_ref.global_stack.pop().unwrap();
    let slot = fn_context_ref.local_data.get(pops.slot_id).unwrap();

    let mut slot_ref = slot.borrow_mut();
    slot_ref.value = data;
}

pub fn replace_stack_top(exec_context: &Rc<RefCell<ExecutionContext>>) {
    let mut exec_context_ref = exec_context.borrow_mut();

    let target = exec_context_ref.global_stack.pop().unwrap().to_owned();
    let source = exec_context_ref.global_stack.pop().unwrap();

    target.replace(source.borrow().to_owned());
    exec_context_ref.global_stack.push(target);
}
