use arc_shared::ginkgo::*;
use std::collections::{BTreeMap, BTreeSet};

mod display;
mod instruction;
mod reader;
#[cfg(test)]
mod tests;

pub(crate) use display::print;
use reader::Reader;

pub fn is_ginkgo(stream: &[u8]) -> bool {
    stream.starts_with(b"GINKGO\0\0")
}

/// Decode Circle's version 2 package without interpreting Ginkgo as legacy bytecode.
pub fn decode(stream: &[u8]) -> Result<Package, String> {
    let mut reader = Reader {
        bytes: stream,
        pos: 0,
    };
    if reader.take(8)? != b"GINKGO\0\0" {
        return Err("Invalid Ginkgo magic".into());
    }
    let version = reader.u32()?;
    if version != 2 {
        return Err(format!("Unsupported Ginkgo format version: {}", version));
    }
    let value = reader.u8()?;
    let package_type =
        PackageType::from_repr(value).ok_or_else(|| format!("Unknown package type: {}", value))?;
    let name_length = reader.length()?;
    let name = std::str::from_utf8(reader.take(name_length)?)
        .map_err(|e| e.to_string())?
        .to_owned();
    let descriptor = PackageDescriptor {
        package_type,
        name,
        version: reader.i64()?,
        entrypoint_function_id: reader.u32()?,
        data_alignment_length: reader.u64()?,
        root_function_table_entry_position: reader.u64()?,
        root_constant_table_entry_position: reader.u64()?,
        root_group_table_entry_position: reader.u64()?,
        region_table_entry_position: reader.u64()?,
    };
    let mut symbols = BTreeMap::new();
    for _ in 0..reader.u32()? {
        let id = reader.u32()?;
        if symbols.insert(id, decode_symbol(&mut reader)?).is_some() {
            return Err(format!("Duplicate symbol ID: {}", id));
        }
    }
    let mut constant_pools = Vec::new();
    for _ in 0..reader.u32()? {
        let mut constants = BTreeMap::new();
        for _ in 0..reader.u32()? {
            let id = reader.u32()?;
            let type_id = reader.u32()?;
            let is_array = reader.bool()?;
            let length = reader.length()?;
            let constant = Constant {
                type_id,
                is_array,
                raw_value: reader.take(length)?.to_vec(),
            };
            if constants.insert(id, constant).is_some() {
                return Err(format!("Duplicate constant index: {}", id));
            }
        }
        constant_pools.push(constants);
    }
    let code = &stream[reader.pos..];
    let mut code_reader = Reader {
        bytes: code,
        pos: 0,
    };
    let mut instructions = Vec::new();
    while code_reader.pos < code.len() {
        instructions.push(instruction::decode(&mut code_reader)?);
    }
    let package = Package {
        descriptor,
        symbols,
        constant_pools,
        instructions,
    };
    validate_functions(&package, code.len())?;
    Ok(package)
}

fn declaration(reader: &mut Reader<'_>) -> Result<Declaration, String> {
    Ok(Declaration {
        dimension: reader.i32()?,
        mutable: reader.bool()?,
        type_id: reader.u32()?,
        allow_none: reader.bool()?,
    })
}

fn decode_symbol(reader: &mut Reader<'_>) -> Result<Symbol, String> {
    let value = reader.u8()?;
    let kind = SymbolKind::from_repr(value)
        .ok_or_else(|| format!("Unknown symbol kind: 0x{:02X}", value))?;
    Ok(match kind {
        SymbolKind::Namespace => Symbol::Namespace,
        SymbolKind::Function => {
            let code_offset = reader.nonnegative()?;
            let code_length = reader.nonnegative()?;
            let return_declaration = declaration(reader)?;
            let parameters = (0..reader.u32()?)
                .map(|_| declaration(reader))
                .collect::<Result<_, _>>()?;
            Symbol::Function(Function {
                code_offset,
                code_length,
                return_declaration,
                parameters,
                annotation_ids: reader.ids()?,
                generic_type_ids: reader.ids()?,
                register_count: reader.nonnegative()?,
                constant_pool: reader.u32()?,
            })
        }
        SymbolKind::GroupField => Symbol::GroupField(declaration(reader)?),
        SymbolKind::DataType => {
            let value = reader.u8()?;
            let category = DataTypeCategory::from_repr(value)
                .ok_or_else(|| format!("Unknown type category: {}", value))?;
            Symbol::DataType(match category {
                DataTypeCategory::Internal => None,
                DataTypeCategory::Complex => Some(reader.u32()?),
            })
        }
        SymbolKind::Group => Symbol::Group {
            field_ids: reader.ids()?,
            lifecycle_functions: (0..reader.u32()?)
                .map(|_| {
                    let value = reader.u8()?;
                    let kind = LifecycleKind::from_repr(value)
                        .ok_or_else(|| format!("Unknown lifecycle kind: {}", value))?;
                    Ok((kind, reader.u32()?))
                })
                .collect::<Result<_, String>>()?,
            function_ids: reader.ids()?,
            subgroup_ids: reader.ids()?,
            annotation_ids: reader.ids()?,
            generic_type_ids: reader.ids()?,
            derivation_type_ids: reader.ids()?,
        },
        SymbolKind::Annotation => Symbol::Annotation(reader.u32()?),
        SymbolKind::Enum => Symbol::Enum {
            annotation_ids: reader.ids()?,
            member_ids: reader.ids()?,
        },
        SymbolKind::EnumMember => Symbol::EnumMember {
            annotation_ids: reader.ids()?,
        },
        SymbolKind::GenericTypeParameter => Symbol::GenericTypeParameter(reader.u32()?),
    })
}

fn validate_functions(package: &Package, code_length: usize) -> Result<(), String> {
    let boundaries: BTreeSet<_> = package
        .instructions
        .iter()
        .map(|i| i.offset)
        .chain([code_length])
        .collect();
    let mut ranges = Vec::new();
    for (&id, symbol) in &package.symbols {
        let Symbol::Function(function) = symbol else {
            continue;
        };
        if function.constant_pool as usize >= package.constant_pools.len() {
            return Err(format!("Invalid constant pool for function {}", id));
        }
        if function.code_length == 0 {
            continue;
        } // External declaration.
        let end = function
            .code_offset
            .checked_add(function.code_length)
            .ok_or("Function range overflow")?;
        if end > code_length
            || !boundaries.contains(&function.code_offset)
            || !boundaries.contains(&end)
        {
            return Err(format!("Invalid code range for function {}", id));
        }
        ranges.push((function.code_offset, end));
        for i in package
            .instructions
            .iter()
            .filter(|i| i.offset >= function.code_offset && i.offset < end)
        {
            for operand in &i.operands {
                if let Operand::Constant(index) = operand {
                    if !package.constant_pools[function.constant_pool as usize].contains_key(index) {
                        return Err(format!(
                            "Invalid constant index {} in pool {} for function {} at {}",
                            index, function.constant_pool, id, i.offset
                        ));
                    }
                }
                if let Operand::Address(target) = operand {
                    let target = usize::try_from(*target)
                        .ok()
                        .and_then(|t| function.code_offset.checked_add(t));
                    if !target.is_some_and(|t| t < end && boundaries.contains(&t)) {
                        return Err(format!(
                            "Invalid jump target in function {} at {}",
                            id, i.offset
                        ));
                    }
                }
            }
        }
    }
    ranges.sort_unstable();
    let mut end = 0;
    for (start, next) in ranges {
        if start != end {
            return Err("Overlapping functions or code outside a function".into());
        }
        end = next;
    }
    if end != code_length {
        return Err("Code outside a function".into());
    }
    Ok(())
}
