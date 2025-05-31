use crate::models::encodings::data_type_enc::DataTypeEncoding;

pub struct SizedArrayEncoding{}

impl SizedArrayEncoding {
    pub fn with_usize_data(data: &[u8]) -> (Vec<usize>, usize) {
        let mut pos = 0;
        let mut result = Vec::new();
        let size = usize::from_le_bytes(data[pos..pos + 8].try_into().unwrap());
        pos += 8;
        for _ in 0..size {
            let value = usize::from_le_bytes(data[pos..pos + 8].try_into().unwrap());
            result.push(value);
            pos += 8;
        }
        (result, pos)
    }

    pub fn with_data_type_data(data: &[u8]) -> (Vec<DataTypeEncoding>, usize) {
        let mut pos = 0;
        let mut result = Vec::new();
        let size = usize::from_le_bytes(data[pos..pos + 8].try_into().unwrap());
        pos += 8;
        for _ in 0..size {
            let (value, len) = DataTypeEncoding::from_u8(&data[pos..]);
            result.push(value);
            pos += len;
        }
        (result, pos)
    }
}
