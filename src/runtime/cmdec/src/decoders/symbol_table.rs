use shared::models::descriptors::symbol::{
    DataTypeSymbol, FunctionSymbol, GroupFieldSymbol, GroupSymbol, NamespaceSymbol, Symbol,
    SymbolDescriptor, SymbolTable,
};
use shared::models::encodings::str_enc::StringEncoding;

pub fn decode_symbol_table(stream: &[u8]) -> (SymbolTable, usize) {
    let mut result = vec![];

    let mut pos = 0;
    let count = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;
    for _ in 0..count {
        let symbol_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        let (symbol, len) = decode_symbol(&stream[pos..]);
        result.push(SymbolDescriptor {
            id: symbol_id,
            value: symbol,
        });
        pos += len;
    }

    (SymbolTable { symbols: result }, pos)
}

fn decode_symbol(stream: &[u8]) -> (Symbol, usize) {
    let result = match stream[0] {
        0x01 => decode_namespace_descriptor(&stream[1..]),
        0x02 => decode_function_descriptor(&stream[1..]),
        0x03 => decode_group_field_descriptor(&stream[1..]),
        0x04 => decode_data_type_descriptor(&stream[1..]),
        0x06 => decode_group_descriptor(&stream[1..]),
        _ => unreachable!(),
    };

    (result.0, result.1 + 1)
}

pub fn decode_function_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let mut pos = 0;
    let entry_pos = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;

    let name_encoding = StringEncoding::from_u8(&stream[pos..]);
    let signature = name_encoding.value;
    pos += name_encoding.total_size;

    (
        Symbol::Function(FunctionSymbol {
            entry_pos,
            signature,
        }),
        pos,
    )
}

pub fn decode_group_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let result = Symbol::Group(GroupSymbol { signature });
    (result, name_encoding.total_size)
}

pub fn decode_group_field_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let result = Symbol::GroupField(GroupFieldSymbol { signature });
    (result, name_encoding.total_size)
}

pub fn decode_data_type_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let result = Symbol::DataType(DataTypeSymbol { signature });
    (result, name_encoding.total_size)
}

pub fn decode_namespace_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let result = Symbol::Namespace(NamespaceSymbol { signature });
    (result, name_encoding.total_size)
}
