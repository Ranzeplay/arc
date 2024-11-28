use std::collections::HashMap;

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum Operator {
    Arithmetic(ArithmeticOperator),
    CompoundAssignment(CompoundAssignmentOperator),
    Bitwise(BitwiseOperator),
    Comparison(ComparisonOperator),
    Logical(LogicalOperator),
    Assignment(AssignmentOperator),
    Malicious(MaliciousOperator),
    Limiter(LimiterOperator),
    Selective(SelectiveOperator)
}

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum ArithmeticOperator {
    Plus,
    Minus,
    Multiply,
    Divide,
    Modulo,
}

pub fn get_arithmetic_operator_map() -> HashMap<&'static str, ArithmeticOperator> {
    use ArithmeticOperator::*;
    let operators = [
        ("+", Plus),
        ("-", Minus),
        ("*", Multiply),
        ("/", Divide),
        ("%", Modulo),
    ];
    operators.iter().cloned().collect()
}

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum CompoundAssignmentOperator {
    SelfIncrement,
    SelfDecrement,
    IncreaseBy,
    DecreaseBy,
    MultiplyBy,
    DivideBy,
    ModBy,
}

pub fn get_compound_assignment_operator_map() -> HashMap<&'static str, CompoundAssignmentOperator> {
    use CompoundAssignmentOperator::*;
    let operators = [
        ("++", SelfIncrement),
        ("--", SelfDecrement),
        ("+=", IncreaseBy),
        ("-=", DecreaseBy),
        ("*=", MultiplyBy),
        ("/=", DivideBy),
        ("%=", ModBy),
    ];
    operators.iter().cloned().collect()
}

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum BitwiseOperator {
    And,
    Or,
    LShift,
    RShift,
    Xor,
    Not,
}

pub fn get_bitwise_operator_map() -> HashMap<&'static str, BitwiseOperator> {
    use BitwiseOperator::*;
    let operators = [
        ("&", And),
        ("|", Or),
        ("<<", LShift),
        (">>", RShift),
        ("^", Xor),
        ("~", Not),
    ];
    operators.iter().cloned().collect()
}

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum ComparisonOperator {
    ObjEq,
    RefEq,
    ObjNeq,
    RefNeq,
    Lt,
    Gt,
    Lte,
    Gte,
}

pub fn get_comparison_operator_map() -> HashMap<&'static str, ComparisonOperator> {
    use ComparisonOperator::*;
    let operators = [
        ("==", ObjEq),
        ("===", RefEq),
        ("<>", ObjNeq),
        ("<!>", RefNeq),
        ("<", Lt),
        (">", Gt),
        ("<=", Lte),
        (">=", Gte),
    ];
    operators.iter().cloned().collect()
}

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum LogicalOperator {
    And,
    Or,
    Not,
}

pub fn get_logical_operator_map() -> HashMap<&'static str, LogicalOperator> {
    use LogicalOperator::*;
    let operators = [
        ("&&", And),
        ("||", Or),
        ("!", Not),
    ];
    operators.iter().cloned().collect()
}

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum AssignmentOperator {
    Assign,
    AssignIfNull,
}

pub fn get_assignment_operator_map() -> HashMap<&'static str, AssignmentOperator> {
    use AssignmentOperator::*;
    let operators = [
        ("=", Assign),
        ("??=", AssignIfNull),
    ];
    operators.iter().cloned().collect()
}

#[derive(Debug, PartialEq, Eq, Hash)]
pub enum MaliciousOperator {
    Question,
    Semicolon,
    Range,
    At,
    Comma,
    Pipe,
}

pub fn get_malicious_operator_map() -> HashMap<&'static str, MaliciousOperator> {
    use MaliciousOperator::*;
    let operators = [
        ("?", Question),
        (";", Semicolon),
        ("..", Range),
        ("@", At),
        (",", Comma),
        ("|>", Pipe),
    ];
    operators.iter().cloned().collect()
}



#[derive(Debug, PartialEq, Eq, Hash)]
pub enum LimiterOperator {
    Dot,
    Arrow,
    OptionalChain,
    Scope,
}

pub fn get_limiter_operator_map() -> HashMap<&'static str, LimiterOperator> {
    use LimiterOperator::*;
    let operators = [
        (".", Dot),
        ("=>", Arrow),
        ("?.", OptionalChain),
        ("::", Scope),
    ];
    operators.iter().cloned().collect()
}



#[derive(Debug, PartialEq, Eq, Hash)]
pub enum SelectiveOperator {
    NullCoalescing,
    Colon,
}

pub fn get_selective_operator_map() -> HashMap<&'static str, SelectiveOperator> {
    use SelectiveOperator::*;
    let operators = [
        ("??", NullCoalescing),
        (":", Colon),
    ];
    operators.iter().cloned().collect()
}
