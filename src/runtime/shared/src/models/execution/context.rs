use crate::models::descriptors::symbol::FunctionSymbol;
use crate::models::execution::data::{DataSlot, DataValue};
use crate::models::instruction::{Instruction, InstructionType};
use crate::models::package::Package;
use std::cell::RefCell;
use std::collections::{BTreeMap, HashMap};
use std::rc::Rc;

pub struct ExecutionContext {
    pub package: Package,
    pub global_stack: Vec<Rc<RefCell<DataValue>>>,
    pub instructions: BTreeMap<usize, Rc<Instruction>>,
    pub jump_destinations: HashMap<usize, usize>,
}

impl ExecutionContext {
    pub fn new(package: Package) -> Self {
        let jump_count = package
            .instructions
            .iter()
            .filter(|instruction| match instruction.instruction_type {
                InstructionType::Jmp(_) => true,
                InstructionType::JmpC(_) => true,
                _ => false,
            })
            .count();

        let instructions = package
            .instructions
            .iter()
            .map(|i| (i.offset, i.clone()))
            .collect();

        ExecutionContext {
            package,
            global_stack: Vec::with_capacity(100),
            jump_destinations: HashMap::with_capacity(jump_count),
            instructions,
        }
    }

    pub fn init_jump_destinations(&mut self) {
        let func = |instruction, offset| {
            let destination = get_jump_destination_instruction_offset(
                &self.package.instructions,
                instruction,
                offset,
            );
            self.package
                .instructions
                .iter()
                .position(|i| i.offset == destination)
                .unwrap()
        };

        for instruction in self.package.instructions.iter() {
            match &instruction.instruction_type {
                InstructionType::Jmp(dest) => {
                    _ = self.jump_destinations.insert(
                        instruction.offset,
                        get_jump_destination_instruction_offset(
                            &self.package.instructions,
                            instruction,
                            dest.jump_offset,
                        ),
                    )
                }
                InstructionType::JmpC(dest) => {
                    _ = self.jump_destinations.insert(
                        instruction.offset,
                        get_jump_destination_instruction_offset(
                            &self.package.instructions,
                            instruction,
                            dest.jump_offset,
                        ),
                    )
                }
                _ => {}
            }
        }
    }
}

pub struct FunctionExecutionContext {
    pub function: Rc<FunctionSymbol>,
    pub local_data: Vec<DataSlot>,
}

pub fn get_jump_destination_instruction_offset(
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
