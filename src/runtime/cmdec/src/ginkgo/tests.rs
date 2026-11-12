use super::*;

fn declaration_bytes() -> Vec<u8> {
    // One dimension, variable, integer, allow none.
    [
        1i32.to_le_bytes().as_slice(),
        &[1],
        &2u32.to_le_bytes(),
        &[1],
    ]
    .concat()
}

fn function(offset: i64, length: i64, pool: u32) -> Vec<u8> {
    let mut bytes = Vec::new();
    bytes.extend(offset.to_le_bytes());
    bytes.extend(length.to_le_bytes());
    bytes.extend(declaration_bytes());
    bytes.extend(1u32.to_le_bytes());
    bytes.extend(declaration_bytes());
    bytes.extend(0u32.to_le_bytes()); // annotations
    bytes.extend(0u32.to_le_bytes()); // generics
    bytes.extend(65_536i64.to_le_bytes());
    bytes.extend(pool.to_le_bytes());
    bytes
}

fn package(
    symbols: &[(u32, u8, Vec<u8>)],
    pools: &[Vec<(u32, u32, Vec<u8>)>],
    code: &[u8],
) -> Vec<u8> {
    let mut bytes = b"GINKGO\0\0".to_vec();
    bytes.extend(2u32.to_le_bytes());
    bytes.push(0); // library
    let name = "测试".as_bytes();
    bytes.extend((name.len() as u64).to_le_bytes());
    bytes.extend(name);
    bytes.extend(1i64.to_le_bytes());
    bytes.extend(0u32.to_le_bytes()); // entrypoint
    bytes.extend([0; 40]); // alignment and reserved positions
    bytes.extend((symbols.len() as u32).to_le_bytes());
    for (id, kind, payload) in symbols {
        bytes.extend(id.to_le_bytes());
        bytes.push(*kind);
        bytes.extend(payload);
    }
    bytes.extend((pools.len() as u32).to_le_bytes());
    for pool in pools {
        bytes.extend((pool.len() as u32).to_le_bytes());
        for (id, type_id, payload) in pool {
            bytes.extend(id.to_le_bytes());
            bytes.extend(type_id.to_le_bytes());
            bytes.push(0);
            bytes.extend((payload.len() as u64).to_le_bytes());
            bytes.extend(payload);
        }
    }
    bytes.extend(code);
    bytes
}

#[test]
fn all_opcodes_have_the_compiler_lengths() {
    let lengths = [
        1, 8, 8, 8, 8, 8, 6, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 8, 8, 8, 1, 9, 12, 5, 4, 4, 4, 2, 8, 6,
        6, 6, 10, 8, 8, 8, 6, 10, 10,
    ];
    let mut code = Vec::new();
    for (opcode, length) in lengths.into_iter().enumerate() {
        code.push(if opcode <= 0x1c {
            opcode as u8
        } else {
            opcode as u8 + 2
        });
        code.extend(vec![0; length - 1]);
    }
    let bytes = package(
        &[(100, 2, function(0, code.len() as i64, 0))],
        &[vec![]],
        &code,
    );
    let decoded = decode(&bytes).unwrap();
    assert_eq!(decoded.instructions.len(), 39);
    let mut offset = 0;
    for (i, length) in decoded.instructions.iter().zip(lengths) {
        assert_eq!(i.offset, offset);
        assert_eq!(i.opcode as u8, i.raw[0]);
        assert_eq!(i.raw.len(), length);
        assert_eq!(
            i.flags.is_none(),
            matches!(
                i.opcode,
                Opcode::NoOperation
                    | Opcode::Label
                    | Opcode::UnconditionalJump
                    | Opcode::PrepareFunctionCall
            )
        );
        offset += length;
    }
}

#[test]
fn operands_keep_full_width_signedness_and_flags() {
    let cases = [
        (
            0x01,
            vec![
                Operand::Register(0x1234),
                Operand::Register(0x5678),
                Operand::Register(0x9abc),
            ],
            vec![0x34, 0x12, 0x78, 0x56, 0xbc, 0x9a],
        ),
        (
            0x17,
            vec![Operand::Symbol(0x12345678)],
            vec![0x78, 0x56, 0x34, 0x12],
        ),
        (0x19, vec![Operand::Register(0xffff)], vec![0xff, 0xff]),
        (
            0x1c,
            vec![Operand::Constant(0x12345678), Operand::Register(256)],
            vec![0x78, 0x56, 0x34, 0x12, 0, 1],
        ),
        (
            0x22,
            vec![
                Operand::Symbol(0x12345678),
                Operand::Register(256),
                Operand::Dimension(300),
            ],
            vec![0x78, 0x56, 0x34, 0x12, 0, 1, 0x2c, 1],
        ),
        (
            0x27,
            vec![
                Operand::Register(256),
                Operand::Register(512),
                Operand::Symbol(0x12345678),
            ],
            vec![0, 1, 0, 2, 0x78, 0x56, 0x34, 0x12],
        ),
    ];
    for (opcode, operands, payload) in cases {
        let mut bytes = vec![opcode];
        bytes.extend(payload);
        if opcode != 0x17 {
            bytes.push(0x1f);
        }
        let mut reader = Reader {
            bytes: &bytes,
            pos: 0,
        };
        let i = instruction::decode(&mut reader).unwrap();
        assert_eq!(i.operands, operands);
        assert_eq!(i.flags, if opcode == 0x17 { None } else { Some(0x1f) });
        assert_eq!(reader.pos, bytes.len());
        // Every operand truncation is an error, rather than a panic or partial result.
        for end in 0..bytes.len() {
            assert!(
                instruction::decode(&mut Reader {
                    bytes: &bytes[..end],
                    pos: 0
                })
                .is_err()
            );
        }
    }
}

#[test]
fn decodes_all_symbol_kinds_and_per_function_constant_pools() {
    let mut group = Vec::new();
    group.extend(1u32.to_le_bytes());
    group.extend(12u32.to_le_bytes());
    group.extend(1u32.to_le_bytes());
    group.push(2);
    group.extend(10u32.to_le_bytes());
    for _ in 0..5 {
        group.extend(0u32.to_le_bytes());
    }
    let symbols = vec![
        (9, 1, vec![]),
        (10, 2, function(0, 2, 0)),
        (11, 2, function(2, 2, 1)),
        (12, 3, declaration_bytes()),
        (13, 4, vec![0]),
        (14, 4, [vec![1], 15u32.to_le_bytes().to_vec()].concat()),
        (15, 6, group),
        (16, 7, 15u32.to_le_bytes().to_vec()),
        (17, 8, vec![0; 8]),
        (18, 9, vec![0; 4]),
        (19, 10, 14u32.to_le_bytes().to_vec()),
    ];
    let pools = vec![
        vec![(0, 5, "你好".as_bytes().to_vec())],
        vec![(0, 3, (-1.25f64).to_le_bytes().to_vec())],
    ];
    let decoded = decode(&package(&symbols, &pools, &[0x1b, 0, 0x1b, 1])).unwrap();
    assert_eq!(decoded.descriptor.package_type, PackageType::Library);
    assert_eq!(decoded.descriptor.name, "测试");
    assert_eq!(decoded.symbols.len(), symbols.len());
    assert!(matches!(decoded.symbols[&13], Symbol::DataType(None)));
    assert!(matches!(decoded.symbols[&14], Symbol::DataType(Some(15))));
    let Symbol::Group {
        lifecycle_functions,
        ..
    } = &decoded.symbols[&15]
    else {
        panic!();
    };
    assert_eq!(lifecycle_functions, &[(LifecycleKind::Destructor, 10)]);
    assert_eq!(decoded.constant_pools[0][&0].raw_value, "你好".as_bytes());
    assert_eq!(
        decoded.constant_pools[1][&0].raw_value,
        (-1.25f64).to_le_bytes()
    );
    let Symbol::Function(f) = &decoded.symbols[&11] else {
        panic!();
    };
    assert_eq!(
        (f.code_offset, f.constant_pool, f.register_count),
        (2, 1, 65_536)
    );
    assert_eq!(f.parameters[0].dimension, 1);
    assert!(f.parameters[0].mutable && f.parameters[0].allow_none);
    assert_eq!(f.parameters[0].type_id, 2);
}

#[test]
fn load_uses_typed_numeric_constants_in_each_function_pool() {
    let code = [0x1c, 0, 0, 0, 0, 0, 1, 3, 0x1b, 0];
    let symbols = [(10, 2, function(0, 10, 0)), (11, 2, function(10, 10, 1))];
    let pools = vec![
        vec![(0, 2, i64::MIN.to_le_bytes().to_vec())],
        vec![(0, 3, (-1.25f64).to_le_bytes().to_vec())],
    ];
    let decoded = decode(&package(&symbols, &pools, &code.repeat(2))).unwrap();
    for instruction in decoded
        .instructions
        .iter()
        .filter(|i| i.opcode == Opcode::Load)
    {
        assert_eq!(
            instruction.operands,
            [Operand::Constant(0), Operand::Register(256)]
        );
        assert_eq!(instruction.flags, Some(3));
    }
    assert_eq!(
        decoded.constant_pools[0][&0].type_id,
        BuiltinType::Integer as u32
    );
    assert_eq!(
        decoded.constant_pools[0][&0].raw_value,
        i64::MIN.to_le_bytes()
    );
    assert_eq!(
        decoded.constant_pools[1][&0].type_id,
        BuiltinType::Decimal as u32
    );
    assert_eq!(
        decoded.constant_pools[1][&0].raw_value,
        (-1.25f64).to_le_bytes()
    );
}

#[test]
fn rejects_corrupt_packages_without_panicking() {
    let bytes = package(
        &[(10, 2, function(0, 2, 0))],
        &[vec![(0, 5, b"hello".to_vec())]],
        &[0x1b, 0],
    );
    for end in 0..bytes.len() {
        assert!(decode(&bytes[..end]).is_err(), "prefix {}", end);
    }
    for version in [1, 3] {
        let mut invalid = bytes.clone();
        invalid[8] = version;
        assert!(decode(&invalid).unwrap_err().contains("version"));
    }
    for opcode in [0x1d, 0x1e] {
        assert!(
            decode(&package(&[], &[], &[opcode]))
                .unwrap_err()
                .contains("opcode")
        );
    }
    let invalid = package(&[(1, 0xff, vec![])], &[], &[]);
    assert!(decode(&invalid).unwrap_err().contains("symbol kind"));
    assert!(
        decode(&package(&[], &[], &[0xff]))
            .unwrap_err()
            .contains("opcode")
    );
    assert!(
        decode(&package(
            &[(1, 2, function(-1, 2, 0))],
            &[vec![]],
            &[0x1b, 0]
        ))
        .is_err()
    );
    assert!(
        decode(&package(
            &[(1, 2, function(0, 2, 1))],
            &[vec![]],
            &[0x1b, 0]
        ))
        .is_err()
    );
    assert!(decode(&package(&[(1, 1, vec![]), (1, 1, vec![])], &[], &[])).is_err());
}

#[test]
fn jumps_are_function_local_and_must_land_on_instruction_starts() {
    let code = [
        vec![0x1b, 0, 0x15],
        0i64.to_le_bytes().to_vec(),
        vec![0x1b, 0],
    ]
    .concat();
    let symbols = [(10, 2, function(0, 2, 0)), (11, 2, function(2, 11, 0))];
    assert!(decode(&package(&symbols, &[vec![]], &code)).is_ok());
    for target in [-1i64, 1, 10, 11, i64::MAX] {
        let mut invalid = code.clone();
        invalid[3..11].copy_from_slice(&target.to_le_bytes());
        assert!(
            decode(&package(&symbols, &[vec![]], &invalid)).is_err(),
            "target {}",
            target
        );
    }
}

#[test]
fn library_with_no_code_or_symbols_is_valid() {
    assert!(
        decode(&package(&[], &[], &[]))
            .unwrap()
            .instructions
            .is_empty()
    );
    // External functions have no instruction bytes.
    assert!(decode(&package(&[(1, 2, function(0, 0, 0))], &[vec![]], &[])).is_ok());
}

#[test]
fn enum_values_follow_the_ginkgo_wire_format() {
    // Keep literal bytes in these fixtures to verify the protocol independently of the enums.
    assert_eq!(Opcode::from_repr(0x01), Some(Opcode::Add));
    assert_eq!(Opcode::from_repr(0x28), Some(Opcode::StoreField));
    assert_eq!(Opcode::from_repr(0x29), None);
    assert_eq!(Opcode::from_repr(0xff), None);
    assert_eq!(Opcode::Load.mnemonic(), "ld");
    assert_eq!(Opcode::from_repr(0x1d), None);
    assert_eq!(Opcode::from_repr(0x1e), None);
    assert_eq!(Opcode::LogicalNot.mnemonic(), "lognot");
    assert_eq!(BuiltinType::from_repr(2), Some(BuiltinType::Integer));
    assert_eq!(BuiltinType::from_repr(8), Some(BuiltinType::Function));
    assert_eq!(BuiltinType::from_repr(9), None);

    let mut bytes = package(&[], &[], &[]);
    bytes[12] = 1;
    assert_eq!(
        decode(&bytes).unwrap().descriptor.package_type,
        PackageType::Executable
    );
    bytes[12] = 2;
    assert!(decode(&bytes).unwrap_err().contains("package type"));

    assert!(
        decode(&package(&[(1, 4, vec![2])], &[], &[]))
            .unwrap_err()
            .contains("type category")
    );
    for (value, expected) in [
        (1, LifecycleKind::Constructor),
        (2, LifecycleKind::Destructor),
        (3, LifecycleKind::DeepCopy),
        (4, LifecycleKind::ShallowCopy),
    ] {
        let group = [
            0u32.to_le_bytes().as_slice(),
            &1u32.to_le_bytes(),
            &[value],
            &10u32.to_le_bytes(),
            &[0; 20],
        ]
        .concat();
        let decoded = decode(&package(&[(1, 6, group)], &[], &[])).unwrap();
        let Symbol::Group {
            lifecycle_functions,
            ..
        } = &decoded.symbols[&1]
        else {
            panic!();
        };
        assert_eq!(lifecycle_functions, &[(expected, 10)]);
    }
    let group = [0u32.to_le_bytes().as_slice(), &1u32.to_le_bytes(), &[0xff]].concat();
    assert!(
        decode(&package(&[(1, 6, group)], &[], &[]))
            .unwrap_err()
            .contains("lifecycle kind")
    );
}
