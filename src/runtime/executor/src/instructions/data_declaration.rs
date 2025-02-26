use std::cell::RefCell;
use std::rc::Rc;
use shared::models::encodings::data_type_enc::{DataTypeEncoding, Mutability};
use shared::models::execution::context::FunctionExecutionContext;
use shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use shared::models::instructions::decl::DeclInstruction;
use shared::models::package::Package;

pub fn declare_data(function_context: &Rc<RefCell<FunctionExecutionContext>>, decl: &DeclInstruction, package: &Package) {
    let mut fn_context_ref = function_context.borrow_mut();
    let slot_id = fn_context_ref.local_data.len();

    let encoding = DataTypeEncoding {
        type_id: decl.data_type_id,
        dimension: decl.dimension,
        mutability: Mutability::Immutable,
        memory_storage_type: decl.memory_storage_type.clone(),
    };

    fn_context_ref.local_data.push(Rc::new(RefCell::new(DataSlot {
        slot_id,
        value: Rc::new(RefCell::new(DataValue {
            data_type: encoding.clone(),
            value: DataValueType::init(Rc::new(encoding), package),
        })),
    })));
}
