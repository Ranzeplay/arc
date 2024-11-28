use crate::models::ast::expression::Expression;
use crate::models::tokens::identifier::Identifier;

pub struct FunctionCall {
    pub name: Identifier,
    pub arguments: Vec<Expression>,
}
