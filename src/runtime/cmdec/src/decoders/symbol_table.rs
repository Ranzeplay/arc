use shared::models::descriptors::symbol::{
    DataTypeSymbol, FunctionSymbol, GroupFieldSymbol, GroupSymbol, NamespaceSymbol, Symbol,
    SymbolDescriptor, SymbolTable,
};
use shared::models::encodings::data_type_enc::{MemoryStorageType, Mutability};
use shared::models::encodings::sized_array_enc::SizedArrayEncoding;
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

    let mut pos = name_encoding.total_size;

    let (field_ids, field_ids_len) = SizedArrayEncoding::with_usize_data(&stream[pos..]);
    pos += field_ids_len;

    let (constructor_ids, constructor_ids_len) = SizedArrayEncoding::with_usize_data(&stream[pos..]);
    pos += constructor_ids_len;

    let (destructor_ids, destructor_ids_len) = SizedArrayEncoding::with_usize_data(&stream[pos..]);
    pos += destructor_ids_len;

    let (function_ids, function_ids_len) = SizedArrayEncoding::with_usize_data(&stream[pos..]);
    pos += function_ids_len;

    let (sub_group_ids, sub_group_ids_len) = SizedArrayEncoding::with_usize_data(&stream[pos..]);
    pos += sub_group_ids_len;

    let result = Symbol::Group(GroupSymbol {
        signature,
        field_ids,
        constructor_ids,
        destructor_ids,
        function_ids,
        sub_group_ids,
    });
    (result, pos)
}

pub fn decode_group_field_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let mut pos = name_encoding.total_size;

    let memory_storage_type = match stream[pos] {
        0x01 => MemoryStorageType::Value,
        0x02 => MemoryStorageType::Reference,
        _ => unreachable!(),
    };
    pos += 1;

    let is_array = stream[pos] == 0x01;
    pos += 1;

    let mutability = match stream[pos] {
        0x01 => Mutability::Mutable,
        0x00 => Mutability::Immutable,
        _ => unreachable!(),
    };
    pos += 1;

    let data_type_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    let result = Symbol::GroupField(GroupFieldSymbol {
        signature,
        memory_storage_type,
        is_array,
        mutability,
        data_type_id,
    });
    (result, pos)
}

pub fn decode_data_type_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let mut pos = 0;
    let type_id = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;

    let name_encoding = StringEncoding::from_u8(&stream[pos..]);
    let signature = name_encoding.value;
    pos += name_encoding.total_size;

    let result = Symbol::DataType(DataTypeSymbol { signature, type_symbol_id: type_id });
    (result, pos)
}

pub fn decode_namespace_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let result = Symbol::Namespace(NamespaceSymbol { signature });
    (result, name_encoding.total_size)
}
