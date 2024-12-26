use shared::models::instruction::{Instruction, InstructionType};
use shared::models::instructions::conditional_jump::ConditionalJumpInstruction;
use shared::models::instructions::decl::DeclInstruction;
use shared::models::instructions::func_call::FunctionCallInstruction;
use shared::models::instructions::jump::JumpInstruction;
use shared::models::instructions::load_stack::LoadStackInstruction;
use shared::models::instructions::pop_to_slot::PopToSlotInstruction;
use shared::models::instructions::return_from_block::ReturnInstruction;
use shared::models::package::Package;
use shared::traits::instruction::DecodableInstruction;

pub fn decode_instructions(stream: &[u8], package: &Package) -> Vec<Instruction> {
    println!("=== Instructions:");

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
            0x05 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::PopD,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
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
            0x0a => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Div,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x0b => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Mod,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x0c => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::LogOr,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x0d => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::LogAnd,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x0e => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::LogNot,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x0f => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BitAnd,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x10 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BitOr,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x11 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BitNot,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x12 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Inv,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x13 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::EqC,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x14 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::EqR,
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
            0x16 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::CLgE,
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
            0x18 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::CLsE,
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
            0x1c => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BTC,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x1d => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BT,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x1e => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BC,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x1f => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BF,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x20 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::ETC,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x21 => {
                let (jump, len) = JumpInstruction::decode(stream, pos, package).unwrap();
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Jmp(jump),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += len;
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
            0x23 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::GType,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x24 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::WAll,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x25 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::TEvt,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x26 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::WEvt,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x28 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::CIR,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x29 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::DIR,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x2a => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Cln,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x2b => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::TermP,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x2c => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::TermEf,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x2d => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::SEf,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x2e => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::PEfId,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x2f => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::CEfId,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x31 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::ShL,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x32 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::ShR,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x33 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::Lbl,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x34 => {
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::BitXor,
                    raw: stream[pos..pos + 1].to_vec(),
                };

                pos += 1;
            }
            0x35 => {
                let (return_function, len) = ReturnInstruction::decode(stream, pos, package).unwrap();
                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::FRet(return_function),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += len;
            }
            0x36 => {
                let (function_call, len) = FunctionCallInstruction::decode(stream, pos, package).unwrap();

                instruction = Instruction {
                    offset: pos,
                    instruction_type: InstructionType::FCall(function_call),
                    raw: stream[pos..pos + len].to_vec(),
                };

                pos += len;
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

    if pos == stream.len() { println!("All instructions decoded successfully!"); }
    else { println!("Remaining instructions: {:02X?}", &stream[pos..stream.len()]); }

    result
}
