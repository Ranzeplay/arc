use crate::models::package::Package;
use crate::traits::instruction::DecodableInstruction;

pub struct LoadStackInstruction {
    pub source: DataSourceType,
    pub location_id: usize,
    pub field: usize,
}

pub enum DataSourceType {
    ConstantTable,
    DataSlot,
    DataHandle,
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
        let source = DataSourceType::from(stream[1]);
        let location_id = usize::from_le_bytes(stream[2..10].try_into().unwrap());
        let field = usize::from_le_bytes(stream[10..18].try_into().unwrap());

        let length = 18;

        Some((
            LoadStackInstruction {
                source,
                location_id,
                field,
            },
            length,
        ))
    }
}
