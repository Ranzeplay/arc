use crate::models::descriptors::symbol::{
    DataTypeSymbol, GroupFieldSymbol, Symbol, SymbolDescriptor,
};
use crate::models::encodings::data_type_enc::{DataTypeEncoding, Mutability};
use crate::models::package::Package;
use std::cell::RefCell;
use std::collections::HashMap;
use std::rc::Rc;

#[derive(Debug)]
pub struct DataSlot {
    pub slot_id: usize,
    pub value: Rc<RefCell<DataValue>>,
}

#[derive(Clone, Debug)]
pub struct DataValue {
    pub data_type: DataTypeEncoding,
    pub value: DataValueType,
}

impl From<&Rc<SymbolDescriptor>> for DataValue {
    fn from(value: &Rc<SymbolDescriptor>) -> Self {
        DataValue {
            data_type: DataTypeEncoding {
                type_id: 0xffffffff,
                dimension: 0,
                mutability: Mutability::Immutable,
            },
            value: DataValueType::Symbol(Rc::clone(value)),
        }
    }
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
    Array(Vec<Rc<RefCell<DataValue>>>),
    Symbol(Rc<SymbolDescriptor>),
}

impl DataValueType {
    pub fn init(data_type_encoding: Rc<DataTypeEncoding>, package: &Package) -> DataValueType {
        if data_type_encoding.dimension > 0 {
            DataValueType::Array(Vec::new())
        } else {
            if data_type_encoding.type_id <= 6 {
                DataValueType::None
            } else {
                DataValueType::Complex(ComplexDataValue::init(&data_type_encoding, package))
            }
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
            .unwrap_or_else(|| panic!("Data type not found: 0x{:016X}", data_type_enc.type_id));
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

        let all_fields = get_all_fields(group_symbol.id, package);

        let mut values: HashMap<usize, Rc<RefCell<DataValue>>> =
            HashMap::with_capacity(all_fields.len());
        for (id, field) in &all_fields {
            let initial_value = DataValue {
                data_type: field.value_descriptor.clone(),
                value: DataValueType::None,
            };

            values.insert(*id, Rc::new(RefCell::new(initial_value)));
        }

        ComplexDataValue {
            data_type: Rc::clone(data_type_enc),
            values,
        }
    }
}

fn get_all_fields(group_id: usize, package: &Package) -> HashMap<usize, Rc<GroupFieldSymbol>> {
    let group = match &package.symbol_table.symbols.get(&group_id).unwrap().value {
        Symbol::Group(g) => g,
        _ => panic!("Symbol 0x{:016X} is not a group", group_id),
    };

    let fields = group
        .field_ids
        .iter()
        .map(|field_id| {
            let field_symbol = package.symbol_table.symbols.get(field_id).unwrap();
            match &field_symbol.value {
                Symbol::GroupField(field) => (*field_id, field.clone()),
                _ => panic!("Field is not a field"),
            }
        })
        .collect::<HashMap<_, _>>();

    let super_types = &group.derivation_type_ids;

    let super_group_fields = super_types
        .iter()
        .map(|t| match &package.symbol_table.symbols.get(t).unwrap().value {
            Symbol::DataType(dt) => match dt.as_ref() {
                DataTypeSymbol::ComplexType(ct) => ct.group_id,
                _ => panic!("Super type is not a complex type"),
            },
            _ => panic!("Super type is not a data type"),
        })
        .flat_map(|id| get_all_fields(id, &package))
        .collect::<HashMap<_, _>>();

    let mut result = HashMap::with_capacity(super_group_fields.len() + fields.len());
    result.extend(super_group_fields);
    result.extend(fields);

    return result;
}
