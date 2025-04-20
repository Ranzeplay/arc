use crate::models::encodings::data_type_enc::MemoryStorageType;
use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;
use std::fmt::{Debug, Formatter};

#[derive(PartialEq, Copy, Clone)]
pub enum DataSourceType {
    ConstantTable,
    DataSlot,
    Field,
    ArrayElement,
    StackTop,
}

impl Debug for DataSourceType {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::ConstantTable => write!(f, "CT"),
            Self::DataSlot => write!(f, "DS"),
            Self::Field => write!(f, "FD"),
            Self::ArrayElement => write!(f, "AE"),
            Self::StackTop => write!(f, "ST"),
        }
    }
}

impl From<u8> for DataSourceType {
    fn from(value: u8) -> Self {
        match value {
            0x01 => Self::ConstantTable,
            0x02 => Self::DataSlot,
            0x03 => Self::Field,
            0x04 => Self::ArrayElement,
            0x05 => Self::StackTop,
            _ => unreachable!(),
        }
    }
}

#[derive(PartialEq, Copy, Clone)]
pub struct StackOperationInstruction {
    pub source: DataSourceType,
    pub storage_type: MemoryStorageType,
    pub location_id: usize,
    pub overwrite: bool,
}

impl Debug for StackOperationInstruction {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        write!(
            f,
            "{:?} {} L:0x{:X} {}",
            self.source,
            match self.storage_type {
                MemoryStorageType::Value => "Val",
                MemoryStorageType::Reference => "Ref",
            },
            self.location_id,
            if self.overwrite { "O" } else { "T" }
        )
    }
}

impl DecodableInstruction<StackOperationInstruction> for StackOperationInstruction {
    fn decode(
        stream: &[u8],
        _offset: usize,
        _package: &Package,
    ) -> Option<(StackOperationInstruction, usize)> {
        let mut pos = 1;
        let source = DataSourceType::from(stream[pos]);
        pos += 1;
        let storage_type = MemoryStorageType::from_u8(stream[pos]);
        pos += 1;
        let location_id = usize::from_le_bytes(stream[pos..pos + 8].try_into().unwrap());
        pos += 8;
        let overwrite = stream[pos] == 0x01;
        pos += 1;

        Some((
            StackOperationInstruction {
                source,
                storage_type,
                location_id,
                overwrite,
            },
            pos,
        ))
    }
}
