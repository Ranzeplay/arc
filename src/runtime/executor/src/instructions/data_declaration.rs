use arc_shared::models::encodings::data_type_enc::{DataTypeEncoding, Mutability};
use arc_shared::models::execution::context::FunctionExecutionContext;
use arc_shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use arc_shared::models::instructions::decl::DeclInstruction;
use arc_shared::models::instructions::new_obj::NewObjectInstruction;
use arc_shared::models::package::Package;
use std::cell::RefCell;
use std::rc::Rc;

pub fn declare_data(function_context: &Rc<RefCell<FunctionExecutionContext>>, decl: &DeclInstruction, package: &Package) {
    let mut fn_context_ref = function_context.borrow_mut();
    let slot_id = fn_context_ref.local_data.len();

    let encoding = DataTypeEncoding {
        type_id: decl.data_type_id,
        dimension: decl.dimension,
        mutability: Mutability::Immutable,
    };

    fn_context_ref.local_data.push(Rc::new(RefCell::new(DataSlot {
        slot_id,
        value: Rc::new(RefCell::new(DataValue {
            data_type: encoding.clone(),
            value: DataValueType::init(Rc::new(encoding), package),
        })),
    })));
}

pub fn construct_data(new_obj: &NewObjectInstruction, package: &Package) -> Rc<RefCell<DataValue>> {
    let encoding = DataTypeEncoding {
        type_id: new_obj.type_id,
        dimension: 0,
        mutability: Mutability::Immutable,
    };

    Rc::new(RefCell::new(DataValue {
        data_type: encoding.clone(),
        value: DataValueType::init(Rc::new(encoding), package),
    }))
}
