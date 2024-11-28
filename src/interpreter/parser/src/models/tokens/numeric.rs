pub fn get_binary_regex() -> String {
    r"^0b[01]+$".to_string()
}

pub fn get_octal_regex() -> String {
    r"^0o[0-7]+$".to_string()
}

pub fn get_decimal_regex() -> String {
    r"^[+-]?(\d+(\.\d*)?|\.\d+)([eE][+-]?\d+)?$".to_string()
}

pub fn get_hexadecimal_regex() -> String {
    r"^0x[0-9A-Fa-f]+$".to_string()
}
