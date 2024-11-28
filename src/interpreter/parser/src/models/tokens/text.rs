pub fn get_string_regex() -> String {
    r#""' (~["\r\n\\] | '\\' .)* '""#.to_string()
}

pub fn get_char_regex() -> String {
    r#"'" (~["\r\n\\] | '\\' .) "'"#.to_string()
}
