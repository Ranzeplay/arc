use crate::models::descriptors::symbol::{FunctionSymbol, Symbol};
use crate::models::execution::data::{DataSlot, DataValue};
use crate::models::instruction::{Instruction, InstructionType};
use crate::models::package::Package;
use std::cell::RefCell;
use std::collections::HashMap;
use std::rc::Rc;

pub struct ExecutionContext {
    pub package: Rc<Package>,
    pub global_stack: Vec<Rc<RefCell<DataValue>>>,
    pub jump_destinations: HashMap<usize, usize>,
    pub function_entry_points: HashMap<usize, usize>,
    pub launch_args: Vec<String>,
}

impl ExecutionContext {
    pub fn new(package: Rc<Package>, launch_args: Vec<String>) -> Self {
        let jump_count = package
            .instructions
            .iter()
            .filter(|instruction| match instruction.instruction_type {
                InstructionType::Jmp(_) => true,
                InstructionType::JmpC(_) => true,
                _ => false,
            })
            .count();

        let function_count = package
            .symbol_table
            .symbols
            .values()
            .filter(|symbol| match symbol.value {
                Symbol::Function(_) => true,
                _ => false,
            })
            .count();

        ExecutionContext {
            package,
            global_stack: Vec::with_capacity(100),
            jump_destinations: HashMap::with_capacity(jump_count),
            function_entry_points: HashMap::with_capacity(function_count),
            launch_args
        }
    }

    pub fn init_jump_destinations(&mut self) {
        for (index, instruction) in self.package.instructions.iter().enumerate() {
            match &instruction.instruction_type {
                InstructionType::Jmp(dest) => {
                    _ = self.jump_destinations.insert(
                        index,
                        get_jump_destination_instruction_index(
                            &self.package.instructions,
                            instruction,
                            dest.jump_offset,
                        ),
                    )
                }
                InstructionType::JmpC(dest) => {
                    _ = self.jump_destinations.insert(
                        index,
                        get_jump_destination_instruction_index(
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

    pub fn init_function_entry_points(&mut self) {
        for symbol in self.package.symbol_table.symbols.values() {
            match &symbol.value {
                Symbol::Function(f) => {
                    let entrypoint_instruction_index = self
                        .package
                        .instructions
                        .iter()
                        .position(|i| i.offset == f.entry_pos)
                        .unwrap_or(0);

                    self.function_entry_points.insert(symbol.id, entrypoint_instruction_index);
                }
                _ => {}
            }
        }
    }
}

pub struct FunctionExecutionContext {
    pub function: Rc<FunctionSymbol>,
    pub local_data: Vec<Rc<RefCell<DataSlot>>>,
}

pub fn get_jump_destination_instruction_index(
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

        instructions.iter().position(|i| i.offset == target_instruction.offset).unwrap()
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

        instructions.iter().position(|i| i.offset == target_instruction.offset).unwrap()
    }
}
