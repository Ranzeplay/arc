use std::fmt::{Debug, Formatter};
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct LoadStackInstruction {
    pub source: DataSourceType,
    pub location_id: usize,
    pub fields: Vec<usize>,
}

impl Debug for LoadStackInstruction {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "{:?} L:0x{:X} F:{}", self.source, self.location_id, self.fields.len())
    }
}

pub struct SaveStackInstruction {
    pub source: DataSourceType,
    pub location_id: usize,
    pub fields: Vec<usize>,
}

impl Debug for SaveStackInstruction {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(f, "{:?} L:0x{:X} F:{}", self.source, self.location_id, self.fields.len())
    }
}

pub enum DataSourceType {
    ConstantTable,
    DataSlot,
    DataHandle,
}

impl Debug for DataSourceType {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::ConstantTable => write!(f, "CT"),
            Self::DataSlot => write!(f, "DS"),
            Self::DataHandle => write!(f, "DH"),
        }
    }
}

impl From<u8> for DataSourceType {
    fn from(value: u8) -> Self {
        match value {
            0x01 => Self::ConstantTable,
            0x02 => Self::DataSlot,
            0x03 => Self::DataHandle,
            _ => unreachable!()
        }
    }
}

impl DecodableInstruction<LoadStackInstruction> for LoadStackInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(LoadStackInstruction, usize)> {
        let mut pos = 1;
        let source = DataSourceType::from(stream[pos]);
        pos += 1;
        let location_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        let field_list_length = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        let mut fields = Vec::with_capacity(field_list_length);
        for _ in 0..field_list_length {
            fields.push(usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap()));
            pos += 8;
        }

        Some((
            LoadStackInstruction {
                source,
                location_id,
                fields,
            },
            pos,
        ))
    }
}

impl DecodableInstruction<SaveStackInstruction> for SaveStackInstruction {
    fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(SaveStackInstruction, usize)> {
        let mut pos = 1;
        let source = DataSourceType::from(stream[pos]);
        pos += 1;
        let location_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        let field_list_length = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        let mut fields = Vec::with_capacity(field_list_length);
        for _ in 0..field_list_length {
            fields.push(usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap()));
            pos += 8;
        }

        Some((
            SaveStackInstruction {
                source,
                location_id,
                fields,
            },
            pos,
        ))
    }
}
