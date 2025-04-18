use lazy_static::lazy_static;

lazy_static!(
    pub static ref NONE_TYPE_ID: usize = 0;
    pub static ref ANY_TYPE_ID: usize = 1;
    pub static ref INTEGER_TYPE_ID: usize = 2;
    pub static ref DECIMAL_TYPE_ID: usize = 3;
    pub static ref CHAR_TYPE_ID: usize = 4;
    pub static ref STRING_TYPE_ID: usize = 5;
    pub static ref BOOLEAN_TYPE_ID: usize = 6;
);
