#![allow(unused)]
use crate::models::instructions::pop_to_slot::PopToSlotInstruction;
use crate::models::instructions::stack_data_operation::StackOperationInstruction;
use crate::models::instructions::return_from_block::ReturnInstruction;
use crate::models::instructions::new_obj::NewObjectInstruction;
use crate::models::instructions::jump::JumpInstruction;
use crate::models::instructions::func_call::FunctionCallInstruction;
use crate::models::instructions::conditional_jump::ConditionalJumpInstruction;
use crate::traits::instruction::DecodableInstruction;
use crate::models::instructions::decl::DeclInstruction;
use crate::models::instructions::return_function::ReturnFunctionInstruction;
use crate::models::instructions::single_instructions::*;

use crate::models::package::Package;
use arc_instruction_factory::generate_instruction_enum;

generate_instruction_enum!(InstructionType);
