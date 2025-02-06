use crate::decoders::constant_table::decode_constant_table;
use crate::decoders::instruction::decode_instructions;
use crate::decoders::package_descriptor::decode_package_descriptor;
use crate::decoders::symbol_table::decode_symbol_table;
use shared::models::descriptors::symbol::Symbol;
use shared::models::display::group::{GroupDetailViewModel, GroupListViewModel};
use shared::models::package::Package;

mod decoders;

pub struct Cmdec {}

impl Cmdec {
    pub fn decode(stream: &[u8], print_decoding_result: bool, verbose: bool) -> Package {
        // Check first 2 bytes
        if stream[0] != 0x20 || stream[1] != 0x24 {
            panic!("Invalid Arc package");
        }

        if print_decoding_result && verbose {
            println!("Raw:\n{:02X?}", stream);
        }

        let mut pos = 2;
        let (package_descriptor, len) = decode_package_descriptor(&stream[pos..]);
        pos += len;
        if print_decoding_result {
            println!("{:?}", package_descriptor);
        }

        let (symbol_table, len) = decode_symbol_table(&stream[pos..]);
        pos += len;
        if print_decoding_result {
            println!("{:?}", symbol_table);
        }

        let mut group_detail_list = GroupListViewModel { groups: Vec::new() };
        for symbol in &symbol_table.symbols {
            match &symbol.value {
                Symbol::Group(group) => {
                    let g = group.clone();
                    group_detail_list.groups.push(GroupDetailViewModel::new(g));
                }
                _ => {}
            }
        }
        if print_decoding_result {
            println!("{:?}", group_detail_list);
        }

        let (constant_table, len) = decode_constant_table(&stream[pos..]);
        pos += len;
        if print_decoding_result {
            println!("{:?}", constant_table);
        }

        let mut package = Package {
            descriptor: package_descriptor,
            symbol_table: symbol_table,
            constant_table: constant_table,
            instructions: Vec::new(),
        };

        package.instructions = decode_instructions(&stream[pos..], &package, verbose);
        package.instructions = decode_instructions(&stream[pos..], &package, print_decoding_result);

        package
    }
}
