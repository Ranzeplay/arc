use super::reader::Reader;
use arc_shared::ginkgo::{Instruction, Operand};

pub(super) fn decode(reader: &mut Reader<'_>) -> Result<Instruction, String> {
    use Operand::*;
    let offset = reader.pos;
    let opcode = reader.u8()?;
    let mnemonic = match opcode {
        0x00 => "nop",
        0x01 => "add",
        0x02 => "sub",
        0x03 => "mul",
        0x04 => "div",
        0x05 => "mod",
        0x06 => "inv",
        0x07 => "lsa",
        0x08 => "rsa",
        0x09 => "rsl",
        0x0a => "bitand",
        0x0b => "bitor",
        0x0c => "bitxor",
        0x0d => "bitnot",
        0x0e => "eq",
        0x0f => "neq",
        0x10 => "lt",
        0x11 => "le",
        0x12 => "gt",
        0x13 => "ge",
        0x14 => "lbl",
        0x15 => "jmp",
        0x16 => "cjp",
        0x17 => "pcall",
        0x18 => "arg",
        0x19 => "invoke",
        0x1a => "retv",
        0x1b => "retn",
        0x1c => "ld_const",
        0x1d => "ld_int",
        0x1e => "ld_float",
        0x1f => "copy",
        0x20 => "clone",
        0x21 => "ref",
        0x22 => "init_obj",
        0x23 => "logand",
        0x24 => "logor",
        0x25 => "logxor",
        0x26 => "lognot",
        0x27 => "ld_field",
        0x28 => "st_field",
        _ => {
            return Err(format!(
                "Unknown opcode 0x{:02X} at instruction offset {}",
                opcode, offset
            ));
        }
    };
    let operands = match opcode {
        0x00 | 0x14 | 0x1b => vec![],
        0x01..=0x05 | 0x07..=0x0c | 0x0e..=0x13 | 0x23..=0x25 => vec![
            Register(reader.u16()?),
            Register(reader.u16()?),
            Register(reader.u16()?),
        ],
        0x06 | 0x0d | 0x1f..=0x21 | 0x26 => vec![Register(reader.u16()?), Register(reader.u16()?)],
        0x15 => vec![Address(reader.i64()?)],
        0x16 => vec![Register(reader.u16()?), Address(reader.i64()?)],
        0x17 => vec![Symbol(reader.u32()?)],
        0x18..=0x1a => vec![Register(reader.u16()?)],
        0x1c => vec![Constant(reader.u32()?), Register(reader.u16()?)],
        0x1d => vec![Integer(reader.i64()?), Register(reader.u16()?)],
        0x1e => vec![Float(reader.f64()?), Register(reader.u16()?)],
        0x22 => vec![
            Symbol(reader.u32()?),
            Register(reader.u16()?),
            Dimension(reader.u16()?),
        ],
        0x27 | 0x28 => vec![
            Register(reader.u16()?),
            Register(reader.u16()?),
            Symbol(reader.u32()?),
        ],
        _ => unreachable!(),
    };
    let flags = if matches!(opcode, 0x00 | 0x14 | 0x15 | 0x17) {
        None
    } else {
        Some(reader.u8()?)
    };
    Ok(Instruction {
        offset,
        opcode,
        mnemonic,
        operands,
        flags,
        raw: reader.bytes[offset..reader.pos].to_vec(),
    })
}
