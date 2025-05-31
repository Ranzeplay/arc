use std::rc::Rc;
use crate::models::descriptors::constant::ConstantTable;
use crate::models::descriptors::package::PackageDescriptor;
use crate::models::descriptors::symbol::SymbolTable;
use crate::models::instruction::Instruction;

#[derive(Debug, Clone)]
pub struct Package {
    pub descriptor: PackageDescriptor,
    pub symbol_table: SymbolTable,
    pub constant_table: ConstantTable,
    pub instructions: Vec<Rc<Instruction>>,
}
