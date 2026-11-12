use arc_shared::ginkgo::*;
use arc_shared::models::options::cmdec_options::CmdecOptions;
use log::info;

pub(crate) fn print(package: &Package, opt: &CmdecOptions, source: Option<&SourceInfo>) {
    if opt.print_package_descriptor {
        info!("Ginkgo format version 2\n{:#?}", package.descriptor);
    }
    if opt.print_symbols {
        info!("=== Symbols");
        for (id, symbol) in &package.symbols {
            info!("{}: {:?}", symbol_name(*id, source), symbol);
        }
    }
    if opt.print_constants {
        for (pool, constants) in package.constant_pools.iter().enumerate() {
            info!("=== Constant pool {}", pool);
            for (id, constant) in constants {
                info!(
                    "{}: type {}{} = {}",
                    id,
                    constant.type_id,
                    if constant.is_array { "[]" } else { "" },
                    constant_value(constant)
                );
            }
        }
    }
    if opt.print_instructions {
        info!("=== Instructions (section-relative offsets; function-local jump targets)");
        for instruction in &package.instructions {
            if let Some((id, Symbol::Function(function))) = package.symbols.iter().find(|(_, symbol)| {
                matches!(symbol, Symbol::Function(f) if f.code_length > 0 && f.code_offset == instruction.offset)
            }) {
                info!("Function {}: offset {}, length {}, registers {}, constant pool {}{}",
                    symbol_name(*id, source), function.code_offset, function.code_length,
                    function.register_count, function.constant_pool,
                    source.and_then(|s| s.function_sources.get(id)).map(|s| format!(", source {}", s)).unwrap_or_default());
            }
            let operands = instruction
                .operands
                .iter()
                .map(|operand| match operand {
                    Operand::Register(id) => format!("r{}", id),
                    Operand::Symbol(id) => symbol_name(*id, source),
                    Operand::Constant(id) => format!("const[{}]", id),
                    Operand::Integer(value) => value.to_string(),
                    Operand::Float(value) => format!("{:?}", value),
                    Operand::Address(value) => format!("local {:#x}", value),
                    Operand::Dimension(value) => format!("dim {}", value),
                })
                .collect::<Vec<_>>()
                .join(", ");
            let flags = instruction
                .flags
                .map(|flags| {
                    format!(
                        " flags=0x{:02X} [{}]",
                        flags,
                        flag_names(instruction.opcode, flags).join(", ")
                    )
                })
                .unwrap_or_default();
            info!(
                "0x{:08X} {:<8} {}{} {:02X?}",
                instruction.offset,
                instruction.opcode.mnemonic(),
                operands,
                flags,
                instruction.raw
            );
        }
    }
}

fn symbol_name(id: u32, source: Option<&SourceInfo>) -> String {
    match source.and_then(|s| s.symbol_mapping.get(&id)) {
        Some(name) => format!("#{} ({})", id, name),
        None => format!("#{}", id),
    }
}

fn flag_names(opcode: Opcode, flags: u8) -> Vec<&'static str> {
    use Opcode::*;
    let names: &[&str] = match opcode {
        Add | Subtract | Multiply | Divide | Modulo | BitwiseAnd | BitwiseOr | BitwiseXor
        | LogicalAnd | LogicalOr | LogicalXor => {
            &["last_use_src_a", "last_use_src_b", "use_dest_next"]
        }
        LeftShiftArithmetic | RightShiftArithmetic | RightShiftLogical => {
            &["last_use_src", "last_use_amount", "use_dest_next"]
        }
        Inverse | BitwiseNot | LogicalNot => &["last_use_src", "use_dest_next"],
        Equal | NotEqual | LessThan | LessThanOrEqual | GreaterThan | GreaterThanOrEqual => &[
            "last_use_src_l",
            "last_use_src_r",
            "use_dest_next",
            "cmp_ref",
        ],
        ConditionalJump => &[
            "cond_true",
            "cond_false",
            "cond_not_none",
            "cond_none",
            "last_use_condition",
        ],
        Argument => &["last_use"],
        Invoke => &["is_virt"],
        ReturnWithValue | ReturnWithoutValue => &["expect_return_value"],
        LoadConstant => &["ld_const_ref", "usage_hint"],
        LoadImmediateInteger | LoadFloatingPointConstant | InitializeObject => &["usage_hint"],
        Copy | Clone | LoadReference | LoadField | StoreField => &["last_use_src", "usage_hint"],
        NoOperation | Label | UnconditionalJump | PrepareFunctionCall => &[],
    };
    names
        .iter()
        .enumerate()
        .filter_map(|(bit, name)| (flags & (1 << bit) != 0).then_some(*name))
        .collect()
}

fn constant_value(constant: &Constant) -> String {
    let bytes = constant.raw_value.as_slice();
    if !constant.is_array {
        match (BuiltinType::from_repr(constant.type_id), bytes.len()) {
            (Some(BuiltinType::None), 0) => return "none".into(),
            (Some(BuiltinType::Integer), 8) => {
                return i64::from_le_bytes(bytes.try_into().unwrap()).to_string();
            }
            (Some(BuiltinType::Decimal), 8) => {
                return format!("{:?}", f64::from_le_bytes(bytes.try_into().unwrap()));
            }
            (Some(BuiltinType::String), _) => {
                if let Ok(text) = std::str::from_utf8(bytes) {
                    return format!("{:?}", text);
                }
            }
            (Some(BuiltinType::Boolean), 1) if bytes[0] <= 1 => return (bytes[0] == 1).to_string(),
            _ => {}
        }
    }
    format!("{:02X?}", bytes)
}
