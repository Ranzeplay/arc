use super::reader::Reader;
use arc_shared::ginkgo::{Instruction, Opcode, Operand};

pub(super) fn decode(reader: &mut Reader<'_>) -> Result<Instruction, String> {
    use Opcode::*;
    use Operand::*;
    let offset = reader.pos;
    let value = reader.u8()?;
    let opcode = Opcode::from_repr(value).ok_or_else(|| {
        format!(
            "Unknown opcode 0x{:02X} at instruction offset {}",
            value, offset
        )
    })?;
    let operands = match opcode {
        NoOperation | Label | ReturnWithoutValue => vec![],
        Add | Subtract | Multiply | Divide | Modulo | LeftShiftArithmetic
        | RightShiftArithmetic | RightShiftLogical | BitwiseAnd | BitwiseOr | BitwiseXor
        | Equal | NotEqual | LessThan | LessThanOrEqual | GreaterThan | GreaterThanOrEqual
        | LogicalAnd | LogicalOr | LogicalXor => vec![
            Register(reader.u16()?),
            Register(reader.u16()?),
            Register(reader.u16()?),
        ],
        Inverse | BitwiseNot | Copy | Clone | LoadReference | LogicalNot => {
            vec![Register(reader.u16()?), Register(reader.u16()?)]
        }
        UnconditionalJump => vec![Address(reader.i64()?)],
        ConditionalJump => vec![Register(reader.u16()?), Address(reader.i64()?)],
        PrepareFunctionCall => vec![Symbol(reader.u32()?)],
        Argument | Invoke | ReturnWithValue => vec![Register(reader.u16()?)],
        LoadConstant => vec![Constant(reader.u32()?), Register(reader.u16()?)],
        LoadImmediateInteger => vec![Integer(reader.i64()?), Register(reader.u16()?)],
        LoadFloatingPointConstant => vec![Float(reader.f64()?), Register(reader.u16()?)],
        InitializeObject => vec![
            Symbol(reader.u32()?),
            Register(reader.u16()?),
            Dimension(reader.u16()?),
        ],
        LoadField | StoreField => vec![
            Register(reader.u16()?),
            Register(reader.u16()?),
            Symbol(reader.u32()?),
        ],
    };
    let flags = if matches!(
        opcode,
        NoOperation | Label | UnconditionalJump | PrepareFunctionCall
    ) {
        None
    } else {
        Some(reader.u8()?)
    };
    Ok(Instruction {
        offset,
        opcode,
        operands,
        flags,
        raw: reader.bytes[offset..reader.pos].to_vec(),
    })
}
