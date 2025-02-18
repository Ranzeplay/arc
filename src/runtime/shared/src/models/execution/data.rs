use crate::models::descriptors::symbol::{DataTypeSymbol, Symbol};
use crate::models::encodings::data_type_enc::DataTypeEncoding;
use std::cell::RefCell;
use std::collections::HashMap;
use std::rc::Rc;
use crate::models::package::Package;

pub struct DataSlot {
    pub slot_id: usize,
    pub value: Rc<RefCell<DataValue>>,
}

#[derive(Clone, Debug)]
pub struct DataValue {
    pub data_type: DataTypeEncoding,
    pub value: DataValueType,
}

#[derive(Clone, Debug)]
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

impl DataValueType {
    pub fn init(data_type_encoding: Rc<DataTypeEncoding>, package: &Package) -> DataValueType {
        if data_type_encoding.type_id <= 6 {
            DataValueType::None
        } else {
            DataValueType::Complex(ComplexDataValue::init(&data_type_encoding, package))
        }
    }
}

#[derive(Clone, Debug)]
pub struct ComplexDataValue {
    pub data_type: Rc<DataTypeEncoding>,
    pub values: HashMap<usize, Rc<RefCell<DataValue>>>,
}

impl ComplexDataValue {
    pub fn init(data_type_enc: &Rc<DataTypeEncoding>, package: &Package) -> ComplexDataValue {
        let data_type_symbol = package
            .symbol_table
            .symbols
            .get(&data_type_enc.type_id)
            .unwrap();
        let data_type = match &data_type_symbol.value {
            Symbol::DataType(dt) => dt,
            _ => panic!("Data type is not a group"),
        };

        let complex_type = match data_type.as_ref() {
            DataTypeSymbol::ComplexType(ct) => ct,
            _ => panic!("Data type is not a complex type"),
        };

        let group_symbol = package
            .symbol_table
            .symbols
            .get(&complex_type.group_id)
            .unwrap();
        let group = match &group_symbol.value {
            Symbol::Group(group) => group,
            _ => panic!("Data type is not a group"),
        };

        let mut values = HashMap::with_capacity(group.field_ids.len());
        for field_id in &group.field_ids {
            let field_symbol = package.symbol_table.symbols.get(field_id).unwrap();
            let field = match &field_symbol.value {
                Symbol::GroupField(field) => field,
                _ => panic!("Field is not a field"),
            };

            let initial_value = DataValue {
                data_type: field.value_descriptor.clone(),
                value: DataValueType::None,
            };

            values.insert(*field_id, Rc::new(RefCell::new(initial_value)));
        }

        ComplexDataValue {
            data_type: Rc::clone(data_type_enc),
            values,
        }
    }
}
