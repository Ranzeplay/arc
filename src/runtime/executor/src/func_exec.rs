use std::cell::RefCell;
use std::rc::Rc;
use shared::models::descriptors::symbol::Symbol;
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use shared::models::instruction::Instruction;

pub struct FunctionInfo {
    pub instruction_slice: Vec<Rc<Instruction>>,
    pub entry_pos: usize,
    pub block_length: usize,
    pub function_context: Rc<RefCell<FunctionExecutionContext>>,
}

pub fn prepare_and_get_function_info(context: Rc<RefCell<ExecutionContext>>, function_id: usize) -> FunctionInfo {
    let mut context_ref = context.borrow_mut();
    let current_fn = context_ref
        .package
        .symbol_table
        .symbols
        .iter()
        .find(|d| d.id == function_id)
        .unwrap();

    let parent_function_opt = if context_ref.stack_frames.len() > 0 {
        Some(context_ref.stack_frames.back().unwrap().clone())
    } else {
        None
    };

    // Create the function context
    let mut function_context = FunctionExecutionContext {
        function: match &current_fn.value {
            Symbol::Function(f) => f.clone(),
            _ => panic!("Invalid symbol type."),
        },
        local_data: vec![],
        local_stack: vec![],
    };

    if let Some(parent_function) = parent_function_opt {
        let mut parent_function = parent_function.borrow_mut();
        let current_function_arg_count = function_context.function.parameter_descriptors.len();
        for _ in 0..current_function_arg_count {
            let data = parent_function.local_stack.pop().unwrap();
            let data = data.borrow();
            let slot = DataSlot {
                slot_id: function_context.local_data.len(),
                value: Rc::new(RefCell::new(data.clone())),
            };
            function_context.local_data.push(slot);
        }
    } else {
        function_context.local_data.push(DataSlot {
            slot_id: 0,
            value: Rc::new(RefCell::new(DataValue {
                data_type: DataTypeEncoding {
                    type_id: 0,
                    is_array: false,
                    mutability: Mutability::Immutable,
                    memory_storage_type: MemoryStorageType::Reference,
                },
                value: DataValueType::None,
            })),
        });
    }

    let entry_pos = function_context.function.entry_pos;
    let block_length = function_context.function.block_length;

    let instruction_slice = context_ref
        .package
        .instructions
        .iter()
        .filter(|&i| i.offset >= entry_pos && i.offset <= entry_pos + block_length)
        .map(|i| i.clone())
        .collect::<Vec<_>>();

    context_ref
        .stack_frames
        .push_back(Rc::new(RefCell::new(function_context)));
    let function_context = context_ref.stack_frames.back().unwrap().clone();

    FunctionInfo {
        instruction_slice,
        entry_pos,
        block_length,
        function_context,
    }
}

#[inline]
pub fn end_function(context: Rc<RefCell<ExecutionContext>>) {
    let mut context_ref = context.borrow_mut();
    context_ref.stack_frames.pop_back();
}
