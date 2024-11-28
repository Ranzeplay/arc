use crate::models::tokens::containers::Container;
use crate::models::tokens::instant::InstantValue;
use crate::models::tokens::keywords::Keyword;
use crate::models::tokens::operators::Operator;

pub enum TokenType {
    Keyword(Keyword),
    Operator(Operator),
    Container(Container),
    Comment(String),
    Instant(InstantValue),
    Invalid
}

pub struct Token {
    pub token_type: TokenType,
    pub value: String,
    pub line: usize,
    pub column: usize,
    pub length: usize,
}
