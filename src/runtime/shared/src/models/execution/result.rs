use crate::models::execution::data::DataValue;

pub enum FunctionExecutionResult<'a> {
    Invalid,
    Success(DataValue<'a>),
    Failure(FunctionExecutionFault),
}

pub struct FunctionExecutionFault {
    pub fault_message: String,
}
