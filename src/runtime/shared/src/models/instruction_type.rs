#![allow(unused)]
use crate::models::instructions::conditional_jump::ConditionalJumpInstruction;
use crate::models::instructions::decl::DeclInstruction;
use crate::models::instructions::func_call::FunctionCallInstruction;
use crate::models::instructions::jump::JumpInstruction;
use crate::models::instructions::new_obj::NewObjectInstruction;
use crate::models::instructions::pop_to_slot::PopToSlotInstruction;
use crate::models::instructions::return_from_block::ReturnInstruction;
use crate::models::instructions::return_function::ReturnFunctionInstruction;
use crate::models::instructions::single_instructions::*;
use crate::models::instructions::stack_data_operation::StackOperationInstruction;
use crate::traits::instruction::DecodableInstruction;

use crate::models::package::Package;
use arc_instruction_factory::generate_instruction_enum;

generate_instruction_enum!(InstructionType);

#[cfg(test)]
mod tests {
    use super::*;
    use crate::models::descriptors::constant::ConstantTable;
    use crate::models::descriptors::package::PackageDescriptor;
    use crate::models::descriptors::symbol::SymbolTable;

    fn empty_package() -> Package {
        Package {
            descriptor: PackageDescriptor::new(),
            symbol_table: SymbolTable {
                symbols: Default::default(),
            },
            constant_table: ConstantTable {
                constants: Default::default(),
            },
            instructions: Vec::new(),
        }
    }

    #[test]
    fn decodes_legacy_opcodes_with_their_original_names_and_lengths() {
        let package = empty_package();
        let names = [
            "Decl", "PushI", "PushC", "PushS", "PopD", "PopS", "Add", "Sub", "Mul", "Div", "Mod",
            "LogOr", "LogAnd", "LogNot", "BitAnd", "BitOr", "BitNot", "Inv", "EqC", "EqR", "CLg",
            "CLgE", "CLs", "CLsE", "", "Ret", "Throw", "BTC", "BT", "BC", "BF", "ETC", "Jmp",
            "JmpC", "GType", "WAll", "TEvt", "WEvt", "CRt", "CIR", "DIR", "Cln", "TermP", "TermEf",
            "SEf", "PEfId", "CEfId", "CType", "ShL", "ShR", "Lbl", "BitXor", "FRet", "FCall",
            "LdStk", "SvStk", "NeqC", "", "", "", "", "", "", "NeqR", "RpStk", "NewObj",
        ];
        for (index, expected_name) in names.iter().enumerate() {
            let opcode = (index + 1) as u8;
            let mut stream = [0u8; 32];
            stream[0] = opcode;
            if matches!(opcode, 0x37 | 0x38) {
                stream[1] = 0x01; // Constant-table data source.
            }
            let decoded = InstructionType::decode(&stream, 0, &package);
            if expected_name.is_empty() {
                assert!(decoded.is_none(), "reserved opcode {opcode:#x}");
                continue;
            }
            let (instruction, length) = decoded.expect("registered legacy opcode");
            assert_eq!(instruction.as_ref(), *expected_name, "opcode {opcode:#x}");
            let expected_length = match opcode {
                0x01 | 0x36 => 21,
                0x06 | 0x21 | 0x22 => 9,
                0x1a | 0x35 => 2,
                0x37 | 0x38 => 11,
                0x42 => 25,
                _ => 1,
            };
            assert_eq!(length, expected_length, "opcode {opcode:#x}");
        }
    }

    #[test]
    fn preserves_signed_jump_operands() {
        let package = empty_package();
        for opcode in [0x21, 0x22] {
            let mut stream = vec![opcode];
            stream.extend_from_slice(&(-123i64).to_le_bytes());
            let (instruction, length) = InstructionType::decode(&stream, 0, &package).unwrap();
            assert_eq!(format!("{instruction:?}"), "L:-123");
            let offset = match instruction {
                InstructionType::Jmp(jump) => jump.jump_offset,
                InstructionType::JmpC(jump) => jump.jump_offset,
                _ => panic!("wrong jump variant"),
            };
            assert_eq!(offset, -123);
            assert_eq!(length, 9);
        }
    }

    #[test]
    fn rejects_empty_and_unknown_instruction_streams() {
        let package = empty_package();
        for stream in [&[][..], &[0x00], &[0xff]] {
            assert!(InstructionType::decode(stream, 0, &package).is_none());
        }
    }
}
