//! Typed views of decoded Ginkgo instructions.
//!
//! Convert with `LoadInstruction::try_from(&instruction)` (or owned `try_into()`).
//! Operands and flags are checked; offsets and raw bytes remain on the original
//! instruction. These conversions do not resolve symbols or unit-pool entries.
//!
//! ```
//! use arc_shared::ginkgo::{Instruction, Opcode, Operand};
//! use arc_shared::ginkgo::instructions::LoadInstruction;
//!
//! let raw = Instruction {
//!     offset: 0, opcode: Opcode::Load,
//!     operands: vec![Operand::Constant(7), Operand::Register(2)],
//!     flags: Some(1), raw: vec![],
//! };
//! let load = LoadInstruction::try_from(&raw)?;
//! assert_eq!(load.constant_index, 7);
//! assert_eq!(load.dest, 2);
//! assert!(load.load_ref);
//! # Ok::<(), arc_shared::ginkgo::instructions::InstructionConversionError>(())
//! ```

use super::{Instruction, Opcode, Operand};
use std::fmt;

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct InstructionConversionError {
    pub expected: Opcode,
    pub actual: Opcode,
    pub reason: &'static str,
}

impl fmt::Display for InstructionConversionError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(
            f,
            "Cannot convert {:?} to {:?}: {}",
            self.actual, self.expected, self.reason
        )
    }
}

impl std::error::Error for InstructionConversionError {}

// The schema drives both concrete conversions and exhaustive typed dispatch.
macro_rules! instructions {
    ($( $name:ident : $opcode:ident {
        $( $field:ident : $ty:ty => $operand:ident ),* $(,)?
    } flags $flags:ident { $( $flag:ident : $bit:literal ),* $(,)? } );* $(;)?) => {
        $(
            #[derive(Debug, Clone, Copy, PartialEq, Eq)]
            pub struct $name {
                $(pub $field: $ty,)*
                $(pub $flag: bool,)*
            }

            impl $name {
                pub const OPCODE: Opcode = Opcode::$opcode;
            }

            impl TryFrom<&Instruction> for $name {
                type Error = InstructionConversionError;

                fn try_from(instruction: &Instruction) -> Result<Self, Self::Error> {
                    let error = |reason| InstructionConversionError {
                        expected: Self::OPCODE, actual: instruction.opcode, reason,
                    };
                    if instruction.opcode != Self::OPCODE {
                        return Err(error("opcode mismatch"));
                    }
                    let [$(Operand::$operand($field)),*] = instruction.operands.as_slice() else {
                        return Err(error("invalid operand count or kind"));
                    };
                    let bits = instructions!(@flags instruction, $flags, error);
                    let mask: u8 = 0 $(| (1 << $bit))*;
                    if bits & !mask != 0 {
                        return Err(error("reserved flag bits are set"));
                    }
                    Ok(Self { $($field: *$field,)* $($flag: bits & (1 << $bit) != 0,)* })
                }
            }

            impl TryFrom<Instruction> for $name {
                type Error = InstructionConversionError;

                fn try_from(instruction: Instruction) -> Result<Self, Self::Error> {
                    Self::try_from(&instruction)
                }
            }
        )*

        #[derive(Debug, Clone, Copy, PartialEq, Eq)]
        pub enum TypedInstruction {
            $($opcode($name),)*
        }

        impl TryFrom<&Instruction> for TypedInstruction {
            type Error = InstructionConversionError;

            fn try_from(instruction: &Instruction) -> Result<Self, Self::Error> {
                match instruction.opcode {
                    $(Opcode::$opcode => Ok(Self::$opcode($name::try_from(instruction)?)),)*
                }
            }
        }

        impl TryFrom<Instruction> for TypedInstruction {
            type Error = InstructionConversionError;

            fn try_from(instruction: Instruction) -> Result<Self, Self::Error> {
                Self::try_from(&instruction)
            }
        }
    };
    (@flags $instruction:ident, Some, $error:ident) => {
        $instruction.flags.ok_or_else(|| $error("missing flags"))?
    };
    (@flags $instruction:ident, None, $error:ident) => {{
        if $instruction.flags.is_some() {
            return Err($error("unexpected flags"));
        }
        0u8
    }};
}

instructions! {
    NoOperationInstruction: NoOperation {} flags None {};
    AddInstruction: Add { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    SubtractInstruction: Subtract { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    MultiplyInstruction: Multiply { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    DivideInstruction: Divide { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    ModuloInstruction: Modulo { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    InverseInstruction: Inverse { src: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, use_dest_next: 1 };
    LeftShiftArithmeticInstruction: LeftShiftArithmetic { src: u16 => Register, amount: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, last_use_amount: 1, use_dest_next: 2 };
    RightShiftArithmeticInstruction: RightShiftArithmetic { src: u16 => Register, amount: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, last_use_amount: 1, use_dest_next: 2 };
    RightShiftLogicalInstruction: RightShiftLogical { src: u16 => Register, amount: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, last_use_amount: 1, use_dest_next: 2 };
    BitwiseAndInstruction: BitwiseAnd { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    BitwiseOrInstruction: BitwiseOr { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    BitwiseXorInstruction: BitwiseXor { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    BitwiseNotInstruction: BitwiseNot { src: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, use_dest_next: 1 };
    EqualInstruction: Equal { src_l: u16 => Register, src_r: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_l: 0, last_use_src_r: 1, use_dest_next: 2, cmp_ref: 3 };
    NotEqualInstruction: NotEqual { src_l: u16 => Register, src_r: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_l: 0, last_use_src_r: 1, use_dest_next: 2, cmp_ref: 3 };
    LessThanInstruction: LessThan { src_l: u16 => Register, src_r: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_l: 0, last_use_src_r: 1, use_dest_next: 2, cmp_ref: 3 };
    LessThanOrEqualInstruction: LessThanOrEqual { src_l: u16 => Register, src_r: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_l: 0, last_use_src_r: 1, use_dest_next: 2, cmp_ref: 3 };
    GreaterThanInstruction: GreaterThan { src_l: u16 => Register, src_r: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_l: 0, last_use_src_r: 1, use_dest_next: 2, cmp_ref: 3 };
    GreaterThanOrEqualInstruction: GreaterThanOrEqual { src_l: u16 => Register, src_r: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_l: 0, last_use_src_r: 1, use_dest_next: 2, cmp_ref: 3 };
    LabelInstruction: Label {} flags None {};
    UnconditionalJumpInstruction: UnconditionalJump { target: i64 => Address } flags None {};
    ConditionalJumpInstruction: ConditionalJump { condition: u16 => Register, target: i64 => Address }
        flags Some { condition_true: 0, condition_false: 1, condition_not_none: 2, condition_none: 3, last_use_condition: 4 };
    PrepareFunctionCallInstruction: PrepareFunctionCall { function_id: u32 => Symbol } flags None {};
    ArgumentInstruction: Argument { src: u16 => Register } flags Some { last_use_src: 0 };
    InvokeInstruction: Invoke { dest: u16 => Register } flags Some { is_virtual: 0 };
    ReturnWithValueInstruction: ReturnWithValue { src: u16 => Register } flags Some { expect_return_value: 0 };
    ReturnWithoutValueInstruction: ReturnWithoutValue {} flags Some { expect_return_value: 0 };
    LoadInstruction: Load { constant_index: u32 => Constant, dest: u16 => Register }
        flags Some { load_ref: 0, usage_hint: 1 };
    CopyInstruction: Copy { src: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, usage_hint: 1 };
    CloneInstruction: Clone { src: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, usage_hint: 1 };
    LoadReferenceInstruction: LoadReference { src: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, usage_hint: 1 };
    InitializeObjectInstruction: InitializeObject { type_id: u32 => Symbol, dest: u16 => Register, dimension: u16 => Dimension }
        flags Some { usage_hint: 0 };
    LogicalAndInstruction: LogicalAnd { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    LogicalOrInstruction: LogicalOr { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    LogicalXorInstruction: LogicalXor { src_a: u16 => Register, src_b: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src_a: 0, last_use_src_b: 1, use_dest_next: 2 };
    LogicalNotInstruction: LogicalNot { src: u16 => Register, dest: u16 => Register }
        flags Some { last_use_src: 0, use_dest_next: 1 };
    LoadFieldInstruction: LoadField { src: u16 => Register, dest: u16 => Register, field_id: u32 => Symbol }
        flags Some { last_use_src: 0, usage_hint: 1 };
    StoreFieldInstruction: StoreField { src: u16 => Register, dest: u16 => Register, field_id: u32 => Symbol }
        flags Some { last_use_src: 0, usage_hint: 1 };
}

#[cfg(test)]
mod tests {
    use super::*;

    fn raw(opcode: Opcode, operands: Vec<Operand>, flags: Option<u8>) -> Instruction {
        Instruction {
            offset: 123,
            opcode,
            operands,
            flags,
            raw: vec![],
        }
    }

    #[test]
    fn named_operands_preserve_width_order_and_signedness() {
        let add: AddInstruction = raw(
            Opcode::Add,
            vec![
                Operand::Register(1),
                Operand::Register(2),
                Operand::Register(u16::MAX),
            ],
            Some(5),
        )
        .try_into()
        .unwrap();
        assert_eq!((add.src_a, add.src_b, add.dest), (1, 2, u16::MAX));
        assert!(add.last_use_src_a && add.use_dest_next);
        assert!(!add.last_use_src_b);

        let load = LoadInstruction::try_from(&raw(
            Opcode::Load,
            vec![Operand::Constant(u32::MAX), Operand::Register(256)],
            Some(2),
        ))
        .unwrap();
        assert_eq!((load.constant_index, load.dest), (u32::MAX, 256));
        assert!(!load.load_ref && load.usage_hint);

        let jump = UnconditionalJumpInstruction::try_from(&raw(
            Opcode::UnconditionalJump,
            vec![Operand::Address(i64::MIN)],
            None,
        ))
        .unwrap();
        assert_eq!(jump.target, i64::MIN);

        let object = InitializeObjectInstruction::try_from(&raw(
            Opcode::InitializeObject,
            vec![
                Operand::Symbol(u32::MAX),
                Operand::Register(2),
                Operand::Dimension(300),
            ],
            Some(1),
        ))
        .unwrap();
        assert_eq!(
            (object.type_id, object.dest, object.dimension),
            (u32::MAX, 2, 300)
        );
        assert!(object.usage_hint);
    }

    #[test]
    fn flags_are_exposed_individually() {
        for bit in 0..5 {
            let jump = ConditionalJumpInstruction::try_from(&raw(
                Opcode::ConditionalJump,
                vec![Operand::Register(9), Operand::Address(42)],
                Some(1 << bit),
            ))
            .unwrap();
            assert_eq!(
                [
                    jump.condition_true,
                    jump.condition_false,
                    jump.condition_not_none,
                    jump.condition_none,
                    jump.last_use_condition
                ],
                std::array::from_fn(|i| i == bit)
            );
        }
        for bit in 0..4 {
            let eq = EqualInstruction::try_from(&raw(
                Opcode::Equal,
                vec![
                    Operand::Register(1),
                    Operand::Register(2),
                    Operand::Register(3),
                ],
                Some(1 << bit),
            ))
            .unwrap();
            assert_eq!(
                [
                    eq.last_use_src_l,
                    eq.last_use_src_r,
                    eq.use_dest_next,
                    eq.cmp_ref
                ],
                std::array::from_fn(|i| i == bit)
            );
        }
    }

    #[test]
    fn malformed_instructions_are_rejected_without_panics() {
        let valid = raw(
            Opcode::Load,
            vec![Operand::Constant(1), Operand::Register(2)],
            Some(0),
        );
        let mut invalid = valid.clone();
        invalid.opcode = Opcode::Copy;
        let error = LoadInstruction::try_from(&invalid).unwrap_err();
        assert_eq!(error.expected, Opcode::Load);
        assert_eq!(error.actual, Opcode::Copy);
        assert!(error.to_string().contains("opcode mismatch"));
        invalid = valid.clone();
        invalid.operands.pop();
        assert!(LoadInstruction::try_from(&invalid).is_err());
        invalid = valid.clone();
        invalid.operands.push(Operand::Register(3));
        assert!(LoadInstruction::try_from(&invalid).is_err());
        invalid = valid.clone();
        invalid.operands[0] = Operand::Symbol(1);
        assert!(LoadInstruction::try_from(&invalid).is_err());
        invalid = valid.clone();
        invalid.flags = None;
        assert!(LoadInstruction::try_from(&invalid).is_err());
        invalid = valid;
        invalid.flags = Some(128);
        assert!(TypedInstruction::try_from(invalid).is_err());
        assert!(
            NoOperationInstruction::try_from(raw(Opcode::NoOperation, vec![], Some(0))).is_err()
        );
        assert!(ReturnWithoutValueInstruction::try_from(raw(
            Opcode::ReturnWithoutValue,
            vec![],
            None
        ))
        .is_err());
    }
}
