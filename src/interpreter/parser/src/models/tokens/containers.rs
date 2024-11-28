use std::collections::HashMap;

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum Container {
    LParen,
    RParen,
    LBracket,
    RBracket,
    LBrace,
    RBrace,
}

pub fn get_container_map() -> HashMap<&'static str, Container> {
    use Container::*;
    let containers = [
        ("(", LParen),
        (")", RParen),
        ("[", LBracket),
        ("]", RBracket),
        ("{", LBrace),
        ("}", RBrace),
    ];
    containers.iter().cloned().collect()
}