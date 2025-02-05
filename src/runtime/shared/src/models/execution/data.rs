use std::cell::RefCell;
use std::rc::Rc;
use crate::models::encodings::data_type_enc::DataTypeEncoding;

pub struct DataSlot {
    pub slot_id: usize,
    pub value: DataValue,
}

pub struct DataValue {
    pub data_type: Rc<DataTypeEncoding>,
    pub value: DataValueType,
}

pub enum DataValueType {
    Bool(bool),
    Integer(i64),
    Decimal(f64),
    String(String),
    Char(char),
    Any,
    None,
    Complex(ComplexDataValue),
}

pub struct ComplexDataValue {
    pub data_type: Rc<DataTypeEncoding>,
    pub values: Vec<Rc<RefCell<DataValue>>>,
}
