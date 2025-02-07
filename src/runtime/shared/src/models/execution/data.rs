use crate::models::encodings::data_type_enc::DataTypeEncoding;
use std::cell::RefCell;
use std::rc::Rc;

pub struct DataSlot {
    pub slot_id: usize,
    pub value: Rc<RefCell<DataValue>>,
}

#[derive(Clone)]
pub struct DataValue {
    pub data_type: DataTypeEncoding,
    pub value: DataValueType,
}

#[derive(Clone)]
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

#[derive(Clone)]
pub struct ComplexDataValue {
    pub data_type: Rc<DataTypeEncoding>,
    pub values: Vec<Rc<RefCell<DataValue>>>,
}
