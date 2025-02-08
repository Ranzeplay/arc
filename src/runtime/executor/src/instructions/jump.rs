use shared::models::instruction::{Instruction, InstructionType};
use std::rc::Rc;

pub fn get_jump_destination(
    instructions: &Vec<Rc<Instruction>>,
    current_instruction: &Rc<Instruction>,
    jump_offset: i64,
) -> usize {
    if jump_offset >= 0 {
        let target_instruction = instructions
            .iter()
            .filter(|i| i.offset > current_instruction.offset)
            .filter(|i| match i.instruction_type {
                InstructionType::Lbl => true,
                _ => false,
            })
            .take(jump_offset as usize)
            .last()
            .unwrap();

        target_instruction.offset
    } else {
        let target_instruction = instructions
            .iter()
            .filter(|i| i.offset < current_instruction.offset)
            .filter(|i| match i.instruction_type {
                InstructionType::Lbl => true,
                _ => false,
            })
            .rev()
            .take(jump_offset.abs() as usize)
            .last()
            .unwrap();

        target_instruction.offset
    }
}
