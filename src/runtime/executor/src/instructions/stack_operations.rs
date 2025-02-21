use crate::data;
use shared::models::encodings::data_type_enc::MemoryStorageType;
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::data::DataValueType;
use shared::models::instructions::pop_to_slot::PopToSlotInstruction;
use shared::models::instructions::stack_data_operation::{
    DataSourceType, StackOperationInstruction
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
            data::get_data_from_data_slot(function_context.clone(), lsi.location_id).to_owned()
        }
        DataSourceType::StackTop => stack.last().unwrap().to_owned(),
        DataSourceType::Field => {
            let stack_top = stack.last().unwrap().to_owned();
            let field_id = lsi.location_id;

            let stack_top_ref = stack_top.borrow();
            let complex_data = match &stack_top_ref.value {
                DataValueType::Complex(d) => d,
                _ => panic!("Invalid data type"),
            };

            Rc::clone(&complex_data.values[&field_id])
        }
        DataSourceType::ArrayElement => {
            let stack_top_index_value = stack.last().unwrap().to_owned();
            let index = match &stack_top_index_value.borrow().value {
                DataValueType::Integer(i) => *i as usize,
                _ => panic!("Invalid data type"),
            };

            let stack_top_array_value = stack.last().unwrap().to_owned();
            let stack_top_array_value = stack_top_array_value.borrow();
            let array = match &stack_top_array_value.value {
                DataValueType::Array(a) => a,
                _ => panic!("Invalid data type"),
            };

            Rc::clone(&array[index])
        }
    };

    let data = match &lsi.storage_type {
        MemoryStorageType::Reference => Rc::clone(&data),
        MemoryStorageType::Value => Rc::new(RefCell::new(data.borrow().to_owned().clone())),
    };

    if lsi.overwrite {
        stack.pop();
    }

    stack.push(data);
}

pub fn save_stack(
    exec_context: &Rc<RefCell<ExecutionContext>>,
    function_context: Rc<RefCell<FunctionExecutionContext>>,
    ssi: &StackOperationInstruction,
) {
    let data = {
        let mut exec_context_ref = exec_context.borrow_mut();
        exec_context_ref.global_stack.pop().unwrap()
    };

    match ssi.source {
        DataSourceType::ConstantTable => {
            panic!("Cannot overwrite the constant table");
        }
        DataSourceType::DataSlot => {
            let data_slot =
                data::get_data_from_data_slot(function_context.clone(), ssi.location_id);

            data_slot.replace(data.borrow().to_owned());
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
    slot.value.replace(data.borrow().to_owned());
}
