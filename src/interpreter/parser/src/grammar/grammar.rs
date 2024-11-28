use lazy_static::lazy_static;
use pest::error::Error;
use pest::iterators::Pairs;
use pest::prec_climber::{Assoc, Operator, PrecClimber};
use pest::Parser;
use pest::pratt_parser::PrattParser;
use pest_derive::Parser;

#[derive(Parser)]
#[grammar = "grammar/base.pest"]
pub struct ArcParser;

pub fn parse(input: &str) -> Result<Pairs<Rule>, Error<Rule>> {
    let result = ArcParser::parse(Rule::SOURCE_FILE, input);
    result
}
