#[derive(Debug)]
pub struct StringEncoding {
    pub value: String,
    pub total_size: usize
}

impl StringEncoding {
    pub fn from_u8(value: &[u8]) -> StringEncoding {
        let size = usize::from_le_bytes(value[0..8].try_into().unwrap());
        let value = &value[8..8 + size];

        let bucket = value.to_vec();

        StringEncoding {
            value: String::from_utf8(bucket).unwrap(),
            total_size: 8 + size
        }
    }
}
