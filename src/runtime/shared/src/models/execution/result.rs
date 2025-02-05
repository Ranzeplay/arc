use std::cell::RefCell;
use std::rc::Rc;
use crate::models::execution::data::DataValue;

pub enum FunctionExecutionResult {
    Invalid,
    Success(Rc<RefCell<DataValue>>),
    Failure(FunctionExecutionFault),
}

pub struct FunctionExecutionFault {
    pub fault_message: String,
}
