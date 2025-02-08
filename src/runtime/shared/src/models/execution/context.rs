use crate::models::descriptors::symbol::{FunctionSymbol, SymbolDescriptor};
use crate::models::execution::data::{DataSlot, DataValue};
use crate::models::package::Package;
use std::cell::RefCell;
use std::collections::VecDeque;
use std::rc::Rc;

pub struct ExecutionContext {
    pub package: Package,
    pub stack_frames: VecDeque<Rc<RefCell<FunctionExecutionContext>>>,
}

impl ExecutionContext {
    pub fn new(package: Package) -> Self {
        ExecutionContext {
            package,
            stack_frames: VecDeque::new(),
        }
    }
}

pub struct FunctionExecutionContext {
    pub function: Rc<FunctionSymbol>,
    pub local_data: Vec<DataSlot>,
    pub local_stack: Vec<Rc<RefCell<DataValue>>>,
}
