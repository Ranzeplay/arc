use crate::models::ast::function_call::FunctionCall;
use crate::models::tokens::identifier::Identifier;
use crate::models::tokens::instant::InstantValue;

pub struct Expression {
    pub terms: Vec<Term>,
}

pub enum Term {
    Instant(InstantValue),
    Variable(Identifier),
    FunctionCall(FunctionCall),
}
