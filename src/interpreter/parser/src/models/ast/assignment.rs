use crate::models::ast::expression::Expression;
use crate::models::tokens::identifier::Identifier;

pub struct Assignment {
    pub target: Identifier,
    pub expression: Expression,
}
