use arc_shared::models::descriptors::package::{PackageDescriptor, PackageType};
use arc_shared::models::encodings::str_enc::StringEncoding;

pub fn decode_package_descriptor(stream: &[u8]) -> (PackageDescriptor, usize) {
    let mut pos: usize = 0;

    let mut result = PackageDescriptor::new();

    // Decode package type
    match stream[0] {
        0x00 => result.package_type = PackageType::Library,
        0x01 => result.package_type = PackageType::Executable,
        _ => unreachable!()
    }
    pos += 1;

    // Decode package name
    let name_encoding = StringEncoding::from_u8(&stream[pos..]);
    result.name = name_encoding.value;
    pos += name_encoding.total_size;

    // Decode version
    result.version = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    // Decode entrypoint function id
    result.entrypoint_function_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    // Decode data alignment length
    result.data_alignment_length = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    // Decode root function table entry pos
    result.root_function_table_entry_position = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    // Decode root constant table entry pos
    result.root_constant_table_entry_position = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    // Decode root group table entry pos
    result.root_group_table_entry_position = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    // Decode region table entry pos
    result.region_table_entry_position = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
    pos += 8;

    (result, pos)
}
