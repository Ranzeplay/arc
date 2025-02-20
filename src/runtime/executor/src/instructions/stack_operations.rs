use crate::data;
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::data::DataValueType;
use shared::models::instructions::pop_to_slot::PopToSlotInstruction;
use shared::models::instructions::stack_data_operation::{DataSourceType, LoadStackInstruction, SaveStackInstruction};
use std::cell::RefCell;
use std::rc::Rc;

pub fn load_stack(
    exec_context: &Rc<RefCell<ExecutionContext>>,
    function_context: Rc<RefCell<FunctionExecutionContext>>,
    lsi: &LoadStackInstruction,
) {
    match lsi.source {
        DataSourceType::ConstantTable => {
            let data = data::get_data_from_constant_table(
                &exec_context.borrow().package,
                lsi.location_id,
                true,
            );
            exec_context
                .borrow_mut()
                .global_stack
                .push(Rc::new(RefCell::new(data)));
        }
        DataSourceType::DataSlot => {
            let data = data::get_data_from_data_slot(function_context.clone(), lsi.location_id);

            exec_context.borrow_mut().global_stack.push(data);
        }
        DataSourceType::StackTop => {},
        DataSourceType::Field => {
            let mut exec_context_ref = exec_context.borrow_mut();
            let stack_top = exec_context_ref.global_stack.pop().unwrap();
            let field_id = lsi.location_id;

            let stack_top_ref = stack_top.borrow();
            let complex_data = match &stack_top_ref.value {
                DataValueType::Complex(d) => d,
                _ => panic!("Invalid data type"),
            };

            let field_data = Rc::clone(&complex_data.values[&field_id]);
            exec_context_ref.global_stack.push(field_data);
        },
        DataSourceType::ArrayElement => {
            let mut exec_context_ref = exec_context.borrow_mut();
            let stack_top_index_value = exec_context_ref.global_stack.pop().unwrap();
            let index = match &stack_top_index_value.borrow().value {
                DataValueType::Integer(i) => *i as usize,
                _ => panic!("Invalid data type"),
            };

            let stack_top_array_value = exec_context_ref.global_stack.pop().unwrap();
            let stack_top_array_value = stack_top_array_value.borrow();
            let array = match &stack_top_array_value.value {
                DataValueType::Array(a) => a,
                _ => panic!("Invalid data type"),
            };

            let element = array[index].clone();
            exec_context_ref.global_stack.push(Rc::new(RefCell::new(element)));
        }
    }
}

pub fn save_stack(
    exec_context: &Rc<RefCell<ExecutionContext>>,
    function_context: Rc<RefCell<FunctionExecutionContext>>,
    ssi: &SaveStackInstruction,
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
            let data_slot = data::get_data_from_data_slot(function_context.clone(), ssi.location_id);

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
