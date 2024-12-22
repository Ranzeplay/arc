use shared::models::instruction::{Instruction, InstructionType};
use shared::models::instructions::conditional_jump::ConditionalJumpInstruction;
use shared::models::instructions::decl::DeclInstruction;
use shared::models::instructions::load_stack::LoadStackInstruction;
use shared::models::instructions::pop_to_slot::PopToSlotInstruction;
use shared::models::instructions::return_from_block::ReturnInstruction;
use shared::models::package::Package;
use shared::traits::instruction::DecodableInstruction;

pub fn decode_instructions(stream: &[u8], package: &Package) -> Vec<Instruction> {
    println!("Instructions: {:02X?}", stream);

    let mut result: Vec<Instruction> = Vec::new();

    let mut pos = 0;
    while pos < stream.len() {
        let instruction: Instruction;
        match stream[pos] {
            0x01 => {
                let (decl, len) = DeclInstruction::decode(stream, pos, package).unwrap();
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Decl(decl),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += len;
            }
            0x06 => {
                let (pop_to_slot, len) = PopToSlotInstruction::decode(stream, pos, package).unwrap();
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::PopS(pop_to_slot),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += len;
            }
            0x07 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Add,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x08 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Sub,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x09 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Mul,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x15 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::CLg,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x17 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::CLs,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x1a => {
                let (return_function, len) = ReturnInstruction::decode(stream, pos, package).unwrap();
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::FRet(return_function),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += 1;
            }
            0x22 => {
                let (cond_jump, len) = ConditionalJumpInstruction::decode(stream, pos, package).unwrap();
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::JmpC(cond_jump),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += len;
            }
            0x35 => {
                let (return_function, len) = ReturnInstruction::decode(stream, pos, package).unwrap();
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::FRet(return_function),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += 1;
            }
            0x37 => {
                let (ldstk, len) = LoadStackInstruction::decode(stream, pos, package).unwrap();

                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::LdStk(ldstk),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += len;
            }
            _ => {
                println!("Unknown instruction: 0x{:02X?} @ {}", stream[pos], pos);
                break;
            }
        }

        println!("{:?}", instruction);
        result.push(instruction);
    }

    println!("Remaining instructions: {:02X?}", &stream[pos..]);

    result
}
