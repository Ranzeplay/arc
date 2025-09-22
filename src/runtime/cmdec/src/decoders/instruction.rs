use arc_shared::models::instruction::{Instruction, InstructionType};
use arc_shared::models::package::Package;
use std::rc::Rc;
use log::{error, info, warn};
use arc_shared::models::descriptors::symbol::Symbol::Function;
use arc_shared::models::options::cmdec_options::CmdecOptions;

pub fn decode_instructions(
    stream: &[u8],
    package: &Package,
    opt: CmdecOptions,
) -> Vec<Rc<Instruction>> {
    if opt.verbose {
        info!("=== Instructions");
    }

    let mut result = vec![];

    let mut pos = 0;
    while pos < stream.len() {
        let i_type = InstructionType::decode(&stream[pos..], pos, package);
        if i_type.is_none() {
            error!("Failed to decode instruction at position: {}", pos);
            break;
        }

        let (i_type, len) = i_type.unwrap();

        let instruction = Instruction {
            instruction_type: i_type,
            offset: 0,
            raw: stream[pos..pos + len].to_vec(),
        };

        if opt.verbose {
            if let Some(symbol) = package.symbol_table.symbols.values().find(|s| match &s.value {
                Function(f) => f.entry_pos == instruction.offset,
                _ => false,
            }) {
                let func = match &symbol.value {
                    Function(f) => f,
                    _ => unreachable!(),
                };

                info!("Function: at {}+{}", func.entry_pos, func.block_length);
            }

            info!("\t{:?}", instruction);
        }

        result.push(Rc::new(instruction));
        pos += len;
    }

    if pos == stream.len() {
        if opt.print_decoding_result {
            info!("All instructions decoded successfully!");
        }
    } else {
        warn!(
            "Remaining instructions: {:02X?}",
            &stream[pos..stream.len()]
        );
    }

    result
}
