use std::collections::HashMap;
use shared::models::descriptors::symbol::{DataTypeSymbol, ComplexTypeSymbol, FunctionSymbol, GroupFieldSymbol, GroupSymbol, NamespaceSymbol, Symbol, SymbolDescriptor, SymbolTable, AnnotationSymbol};
use shared::models::encodings::data_type_enc::DataTypeEncoding;
use shared::models::encodings::sized_array_enc::SizedArrayEncoding;
use shared::models::encodings::str_enc::StringEncoding;
use std::rc::Rc;

pub fn decode_symbol_table(stream: &[u8]) -> (SymbolTable, usize) {
    let mut result = HashMap::new();

    let mut pos = 0;
    let count = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;
    for _ in 0..count {
        let symbol_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;

        let (symbol, len) = decode_symbol(&stream[pos..]);
        result.insert(symbol_id, SymbolDescriptor {
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
        0x07 => decode_annotation_descriptor(&stream[1..]),
        _ => unreachable!(),
    };

    (result.0, result.1 + 1)
}

pub fn decode_function_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let mut pos = 0;
    let entry_pos = usize::from_le_bytes(stream[0..8].try_into().unwrap());
    pos += 8;
    let block_length = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    let name_encoding = StringEncoding::from_u8(&stream[pos..]);
    let signature = name_encoding.value;
    pos += name_encoding.total_size;

    let (return_value_descriptor, return_value_descriptor_len) =
        DataTypeEncoding::from_u8(&stream[pos..]);
    pos += return_value_descriptor_len;

    let (parameter_descriptors, parameter_descriptors_len) =
        SizedArrayEncoding::with_data_type_data(&stream[pos..]);
    pos += parameter_descriptors_len;

    let mut parameter_descriptors_vec: Vec<Rc<DataTypeEncoding>> = vec![];
    let parameter_descriptors_vec_temp: Vec<DataTypeEncoding> = parameter_descriptors.into();
    for d in parameter_descriptors_vec_temp {
        parameter_descriptors_vec.push(Rc::new(d));
    }

    let (annotations, annotation_len) = SizedArrayEncoding::with_usize_data(&stream[pos..]);
    pos += annotation_len;

    (
        Symbol::Function(Rc::new(FunctionSymbol {
            entry_pos,
            block_length,
            signature,
            return_value_descriptor: Rc::new(return_value_descriptor),
            parameter_descriptors: parameter_descriptors_vec,
            annotation_ids: annotations,
        })),
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

    let (annotation_ids, annotation_ids_len) = SizedArrayEncoding::with_usize_data(&stream[pos..]);
    pos += annotation_ids_len;

    let result = Symbol::Group(Rc::new(GroupSymbol {
        signature,
        field_ids,
        constructor_ids,
        destructor_ids,
        function_ids,
        sub_group_ids,
        annotation_ids,
    }));
    (result, pos)
}

pub fn decode_group_field_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let mut pos = name_encoding.total_size;

    let (data_type, data_type_id_len) = DataTypeEncoding::from_u8(&stream[pos..]);
    pos += data_type_id_len;

    let result = Symbol::GroupField(Rc::new(GroupFieldSymbol {
        signature,
        value_descriptor: data_type,
    }));
    (result, pos)
}

pub fn decode_data_type_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let mut pos = 0;

    let is_base_type = stream[pos] == 0x00;
    pos += 1;

    let name_encoding = StringEncoding::from_u8(&stream[pos..]);
    pos += name_encoding.total_size;

    if is_base_type {
        (
            Symbol::DataType(Rc::new(DataTypeSymbol::BaseType(name_encoding.value))),
            pos,
        )
    } else {
        let group_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        (
            Symbol::DataType(Rc::new(DataTypeSymbol::ComplexType(
                ComplexTypeSymbol {
                    signature: name_encoding.value,
                    group_id,
                },
            ))),
            pos,
        )
    }
}

pub fn decode_namespace_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let name_encoding = StringEncoding::from_u8(&stream);
    let signature = name_encoding.value;

    let result = Symbol::Namespace(Rc::new(NamespaceSymbol { signature }));
    (result, name_encoding.total_size)
}

pub fn decode_annotation_descriptor(stream: &[u8]) -> (Symbol, usize) {
    let mut pos = 0;
    let name_encoding = StringEncoding::from_u8(&stream[pos..]);
    let signature = name_encoding.value;
    pos += name_encoding.total_size;

    let group_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    let result = Symbol::Annotation(Rc::new(AnnotationSymbol { signature, group_id }));
    (result, pos)
}
