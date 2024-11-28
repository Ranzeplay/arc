pub enum InstantValue {
    Number(i64),
    Decimal(f64),
    String(String),
    Char(char),
    Boolean(bool),
    None,
    Any,
}
