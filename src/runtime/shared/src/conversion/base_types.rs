use std::cell::RefCell;
use std::rc::Rc;
use crate::base_type_id::{INTEGER_TYPE_ID, STRING_TYPE_ID};
use crate::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use crate::models::execution::data::{DataValue, DataValueType};

impl TryInto<i64> for DataValue {
    type Error = String;

    fn try_into(self) -> Result<i64, Self::Error> {
        match self.value {
            DataValueType::Integer(i) => Ok(i),
            _ => Err("Expected integer".to_string()),
        }
    }
}

impl TryInto<f64> for DataValue {
    type Error = String;

    fn try_into(self) -> Result<f64, Self::Error> {
        match self.value {
            DataValueType::Decimal(d) => Ok(d),
            _ => Err("Expected decimal".to_string()),
        }
    }
}

impl TryInto<String> for DataValue {
    type Error = String;

    fn try_into(self) -> Result<String, Self::Error> {
        match self.value {
            DataValueType::String(s) => Ok(s.clone()),
            _ => Err("Expected string".to_string()),
        }
    }
}

impl TryInto<char> for DataValue {
    type Error = String;

    fn try_into(self) -> Result<char, Self::Error> {
        match self.value {
            DataValueType::Char(c) => Ok(c),
            _ => Err("Expected char".to_string()),
        }
    }
}

impl TryInto<bool> for DataValue {
    type Error = String;

    fn try_into(self) -> Result<bool, Self::Error> {
        match self.value {
            DataValueType::Bool(b) => Ok(b),
            _ => Err("Expected bool".to_string()),
        }
    }
}

impl TryInto<Vec<Rc<RefCell<DataValue>>>> for DataValue {
    type Error = String;

    fn try_into(self) -> Result<Vec<Rc<RefCell<DataValue>>>, Self::Error> {
        match self.value {
            DataValueType::Array(a) => Ok(a.clone()),
            _ => Err("Expected array".to_string()),
        }
    }
}

impl From<String> for DataValue {
    fn from(value: String) -> Self {
        let data_type = DataTypeEncoding {
            type_id: *STRING_TYPE_ID,
            dimension: 0,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::String(value);

        DataValue{ data_type, value }
    }
}

impl From<i64> for DataValue {
    fn from(value: i64) -> Self {
        let data_type = DataTypeEncoding {
            type_id: *INTEGER_TYPE_ID,
            dimension: 0,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::Integer(value);

        DataValue{ data_type, value }
    }
}

impl From<f64> for DataValue {
    fn from(value: f64) -> Self {
        let data_type = DataTypeEncoding {
            type_id: *INTEGER_TYPE_ID,
            dimension: 0,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::Decimal(value);

        DataValue{ data_type, value }
    }
}

impl From<bool> for DataValue {
    fn from(value: bool) -> Self {
        let data_type = DataTypeEncoding {
            type_id: *INTEGER_TYPE_ID,
            dimension: 0,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::Bool(value);

        DataValue{ data_type, value }
    }
}

impl From<char> for DataValue {
    fn from(value: char) -> Self {
        let data_type = DataTypeEncoding {
            type_id: *INTEGER_TYPE_ID,
            dimension: 0,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Value,
        };

        let value = DataValueType::Char(value);

        DataValue{ data_type, value }
    }
}
