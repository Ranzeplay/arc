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
            let mut data = data::get_data_from_data_slot(function_context.clone(), lsi.location_id);

            let len = lsi.fields.len();
            if len > 0 {
                for field_id in &lsi.fields {
                    let next_data = {
                        let data_ref = data.borrow();
                        let complex_data = match &data_ref.value {
                            DataValueType::Complex(d) => d,
                            _ => panic!("Invalid data type"),
                        };
                        Rc::clone(&complex_data.values[field_id])
                    };
                    data = next_data;
                }
            }

            exec_context.borrow_mut().global_stack.push(data);
        }
        DataSourceType::DataHandle => {}
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

            let len = ssi.fields.len();
            if len > 0 {
                let mut current_data = data_slot;
                for field_id in &ssi.fields {
                    let next_data = {
                        let data_ref = current_data.borrow();
                        let complex_data = match &data_ref.value {
                            DataValueType::Complex(d) => d,
                            _ => panic!("Invalid data type"),
                        };
                        Rc::clone(&complex_data.values[field_id])
                    };
                    current_data = next_data;
                }

                current_data.replace(data.borrow().to_owned());

            } else {
                data_slot.replace(data.borrow().to_owned());
            }
        }
        DataSourceType::DataHandle => {}
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
