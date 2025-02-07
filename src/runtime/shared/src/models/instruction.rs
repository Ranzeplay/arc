use crate::models::instructions::conditional_jump::ConditionalJumpInstruction;
use crate::models::instructions::decl::DeclInstruction;
use crate::models::instructions::func_call::FunctionCallInstruction;
use crate::models::instructions::jump::JumpInstruction;
use crate::models::instructions::load_stack::LoadStackInstruction;
use crate::models::instructions::pop_to_slot::PopToSlotInstruction;
use crate::models::instructions::return_from_block::ReturnInstruction;
use std::fmt::{Debug, Formatter};
use pad::PadStr;
use strum_macros::AsRefStr;

#[derive(AsRefStr)]
pub enum InstructionType {
    Decl(DeclInstruction),
    PushI,
    PushC,
    PushS,
    PopD,
    PopS(PopToSlotInstruction),
    Add,
    Sub,
    Mul,
    Div,
    Mod,
    LogOr,
    LogAnd,
    LogNot,
    BitAnd,
    BitOr,
    BitNot,
    Inv,
    EqC,
    EqR,
    CLg,
    CLgE,
    CLs,
    CLsE,
    Invoke(FunctionCallInstruction),
    Ret(ReturnInstruction),
    Throw,
    BTC,
    BT,
    BC,
    BF,
    ETC,
    Jmp(JumpInstruction),
    JmpC(ConditionalJumpInstruction),
    GType,
    WAll,
    TEvt,
    WEvt,
    CRt,
    CIR,
    DIR,
    Cln,
    TermP,
    TermEf,
    SEf,
    PEfId,
    CEfId,
    CType,
    ShL,
    ShR,
    Lbl,
    BitXor,
    FRet(ReturnInstruction),
    FCall(FunctionCallInstruction),
    LdStk(LoadStackInstruction),
    SvStk,
    NeqC,
    NeqR,
}

impl Debug for InstructionType {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        match self {
            InstructionType::Decl(x) => write!(f, "{:?}", x),
            InstructionType::PopS(x) => write!(f, "{:?}", x),
            InstructionType::Ret(x) => write!(f, "{:?}", x),
            InstructionType::Jmp(x) => write!(f, "{:?}", x),
            InstructionType::JmpC(x) => write!(f, "{:?}", x),
            InstructionType::FRet(x) => write!(f, "{:?}", x),
            InstructionType::FCall(x) => write!(f, "{:?}", x),
            InstructionType::LdStk(x) => write!(f, "{:?}", x),
            _ => write!(f, " "),
        }
    }
}

pub struct Instruction {
    pub instruction_type: InstructionType,
    pub offset: usize,
    pub raw: Vec<u8>,
}

impl Debug for Instruction {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        let description = format!("{:?}", self.instruction_type);

        write!(
            f,
            "0x{:08X} {:4} {:<8} {} {:02X?}",
            self.offset,
            ' ',
            self.instruction_type.as_ref(),
            description.pad_to_width(32),
            self.raw
        )
    }
}
