use crate::models::descriptors::symbol::FunctionSymbol;
use crate::models::execution::data::{DataSlot, DataValue};
use crate::models::package::Package;
use std::cell::RefCell;
use std::rc::Rc;

pub struct ExecutionContext {
    pub package: Package,
    pub global_stack: Vec<Rc<RefCell<DataValue>>>,
}

impl ExecutionContext {
    pub fn new(package: Package) -> Self {
        ExecutionContext {
            package,
            global_stack: Vec::with_capacity(100),
        }
    }
}

pub struct FunctionExecutionContext {
    pub function: Rc<FunctionSymbol>,
    pub local_data: Vec<DataSlot>,
}
