use crate::models::descriptors::constant::ConstantTable;
use crate::models::descriptors::package::PackageDescriptor;
use crate::models::descriptors::symbol::SymbolTable;
use crate::models::instruction::InstructionType;

pub struct Package {
    pub descriptor: PackageDescriptor,
    pub symbols: SymbolTable,
    pub constants: ConstantTable,
    pub instructions: Vec<InstructionType>,
}
