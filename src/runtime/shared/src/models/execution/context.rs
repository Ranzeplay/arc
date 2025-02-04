use crate::models::descriptors::symbol::FunctionSymbol;
use crate::models::execution::data::DataSlot;
use crate::models::package::Package;
use std::collections::VecDeque;

pub struct ExecutionContext<'a> {
    pub package: Package,
    pub stack_frames: VecDeque<FunctionExecutionContext<'a>>,
}

impl ExecutionContext<'_> {
    pub fn new(package: Package) -> Self {
        ExecutionContext {
            package,
            stack_frames: VecDeque::new(),
        }
    }
}

pub struct FunctionExecutionContext<'a> {
    pub function: &'a FunctionSymbol,
    pub local_data: Vec<DataSlot<'a>>,
}
