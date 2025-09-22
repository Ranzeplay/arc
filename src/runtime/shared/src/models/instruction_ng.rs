#[allow(unused)]
use crate::models::instructions::single_instructions::*;
#[allow(unused)]
use crate::models::instructions::return_function::ReturnFunctionInstruction;

use std::fmt::Debug;
use arc_instruction_factory::{arc_instruction, generate_instruction_enum};
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

#[derive(PartialEq, Clone, Debug)]
pub struct ExampleInstruction {}

#[arc_instruction(0x01, "ExInstr")]
impl DecodableInstruction<ExampleInstruction> for ExampleInstruction {
    fn decode(stream: &[u8], offset: usize, package: &Package) -> Option<(ExampleInstruction, usize)> {
        panic!()
    }
}

generate_instruction_enum!(InstructionNg);

fn test() {
    let i = InstructionNg::ExInstr(ExampleInstruction{});
}
