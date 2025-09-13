#[derive(Debug, Clone)]
pub enum Mutability {
    Immutable,
    Mutable,
}

impl Mutability {
    pub fn from_u8(value: u8) -> Mutability {
        match value {
            0x01 => Mutability::Mutable,
            0x00 => Mutability::Immutable,
            _ => unreachable!(),
        }
    }
}

#[derive(Debug, Clone)]
pub struct DataTypeEncoding {
    pub type_id: usize,
    pub dimension: u32,
    pub mutability: Mutability,
}

impl DataTypeEncoding {
    pub fn from_u8(stream: &[u8]) -> (DataTypeEncoding, usize) {
        let mut pos = 0;

        let dimension = u32::from_le_bytes(stream[pos..pos + 4].try_into().unwrap());
        pos += 4;

        let mutability = Mutability::from_u8(stream[pos]);
        pos += 1;

        let type_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;

        (
            DataTypeEncoding {
                type_id,
                dimension,
                mutability,
            },
            pos,
        )
    }
}
