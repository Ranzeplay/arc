use std::cell::RefCell;
use std::rc::Rc;
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::instructions::pop_to_slot::PopToSlotInstruction;
use shared::models::instructions::stack_data_operation::{DataSourceType, LoadStackInstruction};
use crate::data;

pub fn load_stack(exec_context: &Rc<RefCell<ExecutionContext>>, function_context: Rc<RefCell<FunctionExecutionContext>>, lsi: &LoadStackInstruction) {
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
            let data =
                data::get_data_from_data_slot(function_context.clone(), lsi.location_id);
            exec_context.borrow_mut().global_stack.push(data);
        }
        DataSourceType::DataHandle => {}
    }
}

pub fn pop_to_slot(exec_context: &Rc<RefCell<ExecutionContext>>, function_context: &Rc<RefCell<FunctionExecutionContext>>, pops: &PopToSlotInstruction) {
    let fn_context_ref = function_context.borrow();
    let mut exec_context_ref = exec_context.borrow_mut();
    let data = exec_context_ref.global_stack.pop().unwrap();
    let slot = fn_context_ref.local_data.get(pops.slot_id).unwrap();
    slot.value.replace(data.borrow().to_owned());
}
