use std::fmt::{Debug, Formatter};
use strum_macros::AsRefStr;
use crate::models::instructions::conditional_jump::ConditionalJumpInstruction;
use crate::models::instructions::decl::DeclInstruction;
use crate::models::instructions::func_call::FunctionCallInstruction;
use crate::models::instructions::jump::JumpInstruction;
use crate::models::instructions::load_stack::LoadStackInstruction;
use crate::models::instructions::pop_to_slot::PopToSlotInstruction;
use crate::models::instructions::return_from_block::ReturnInstruction;

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
    Invoke,
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
    SvStk
}

pub struct Instruction {
    pub instruction_type: InstructionType,
    pub offset: usize,
    pub raw: Vec<u8>,
}

impl Debug for Instruction {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "0x{:08X} {:4} {:<16} {:02X?}", self.offset, ' ', self.instruction_type.as_ref(), self.raw)
    }
}
