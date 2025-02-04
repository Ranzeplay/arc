use crate::models::encodings::data_type_enc::DataTypeEncoding;

pub struct DataSlot<'a> {
    pub slot_id: usize,
    pub value: DataValue<'a>,
}

pub struct DataValue<'a> {
    pub data_type: &'a DataTypeEncoding,
    pub value: Vec<u8>,
}
