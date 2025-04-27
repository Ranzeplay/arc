use std::cell::RefCell;
use std::fmt::{Display, Formatter};
use std::rc::Rc;
use crate::models::execution::data::DataValue;

pub enum FunctionExecutionResult {
    Invalid,
    Success(Option<Rc<RefCell<DataValue>>>),
    Failure(Rc<RefCell<FunctionExecutionFault>>),
}

pub struct FunctionExecutionFault {
    pub stack_trace: Vec<StackTraceLocation>,
    pub exception: Rc<RefCell<DataValue>>,
}

impl Display for FunctionExecutionFault {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "Unhandled exception: {:?}", self.exception.borrow())?;
        writeln!(f, "Stack trace:")?;
        for location in &self.stack_trace {
            writeln!(f, "\t{}", location)?;
        }

        writeln!(f, "End of stack trace")
    }
}

pub struct StackTraceLocation {
    pub instruction_location: usize,
    pub function_id: usize,
}

impl StackTraceLocation {
    pub fn new(instruction_location: usize, function_id: usize) -> Self {
        StackTraceLocation {
            instruction_location,
            function_id,
        }
    }
}

impl Display for StackTraceLocation {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        writeln!(
            f,
            "0x{:016X} @ 0x{:016X}",
            self.instruction_location, self.function_id
        )
    }
}
