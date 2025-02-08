mod data;
mod math;

use log::{debug, trace};
use shared::models::descriptors::symbol::{Symbol, SymbolDescriptor};
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use shared::models::execution::result::FunctionExecutionResult;
use shared::models::instruction::InstructionType;
use shared::models::instructions::load_stack::DataSourceType;
use shared::models::package::Package;
use std::cell::RefCell;
use std::rc::Rc;

pub fn launch(package: Package, _verbose: bool) -> Result<i32, String> {
    debug!("Launching program...");

    let context = ExecutionContext::new(package);
    let context_rc = Rc::new(RefCell::new(context));
    let result = execute(context_rc);

    match result {
        FunctionExecutionResult::Success(_) => {
            debug!("Program executed successfully.");
            Ok(0)
        }
        FunctionExecutionResult::Failure(fault) => {
            debug!("Program execution failed: {}", fault.fault_message);
            Ok(1)
        }
        FunctionExecutionResult::Invalid => Ok(0),
    }
}

pub fn execute(context: Rc<RefCell<ExecutionContext>>) -> FunctionExecutionResult {
    let function_id = {
        let context_ref = context.borrow();
        context_ref.package.descriptor.entrypoint_function_id
    };

    execute_function(function_id, context.clone())
}

pub fn execute_function(
    function_id: usize,
    context: Rc<RefCell<ExecutionContext>>,
) -> FunctionExecutionResult {
    let (instruction_slice, entry_pos, block_length, function_context) = {
        let mut context_ref = context.borrow_mut();
        let entry_function = context_ref
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
            function: match &entry_function.value {
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
                function_context.local_stack.push(data.clone());
            }
        }

        let entry_pos = function_context.function.entry_pos;
        let block_length = function_context.function.block_length;

        let instruction_slice = context_ref
            .package
            .instructions
            .iter()
            .take_while(|i| i.offset >= entry_pos && i.offset <= entry_pos + block_length)
            .map(|i| i.clone())
            .collect::<Vec<_>>();

        context_ref
            .stack_frames
            .push_back(Rc::new(RefCell::new(function_context)));
        let entry_function_context = context_ref.stack_frames.back().unwrap().clone();

        (
            instruction_slice,
            entry_pos,
            block_length,
            entry_function_context,
        )
    };

    {
        if function_id >= 0xa1 && function_id <= 0xff {
            trace!("Executing stdlib function");

            match function_id {
                0xa1 => {
                    // Arc::Std::Console::PrintString
                    let mut fn_context_ref = function_context.borrow_mut();
                    let data = fn_context_ref.local_stack.pop().unwrap();

                    let data = data.borrow();

                    match &data.value {
                        DataValueType::String(s) => {
                            print!("{}", s);
                        }
                        _ => {
                            panic!("Invalid data type")
                        }
                    }
                },
                0xa2 => {
                    // Arc::Std::Console::PrintInteger
                    let mut fn_context_ref = function_context.borrow_mut();
                    let data = fn_context_ref.local_stack.pop().unwrap();

                    let data = data.borrow();

                    match &data.value {
                        DataValueType::Integer(i) => {
                            print!("{}", i);
                        }
                        _ => {
                            panic!("Invalid data type")
                        }
                    }
                },
                _ => {
                    panic!("Unknown stdlib function")
                }
            }
        }
    }

    let mut instruction_offset = entry_pos;
    loop {
        if instruction_offset >= entry_pos + block_length - 1 {
            break;
        }

        let instructions = instruction_slice
            .iter()
            .filter(|i| i.offset >= instruction_offset)
            .take(2)
            .collect::<Vec<_>>();

        let instruction = instructions.first().unwrap();
        let next_instruction = instructions.last().unwrap();

        match &instruction.instruction_type {
            InstructionType::Decl(decl) => {
                let mut fn_context_ref = function_context.borrow_mut();
                let slot_id = fn_context_ref.local_data.len();
                fn_context_ref.local_data.push(DataSlot {
                    slot_id,
                    value: Rc::new(RefCell::new(DataValue {
                        data_type: DataTypeEncoding {
                            type_id: decl.data_type_id,
                            is_array: decl.is_array,
                            mutability: Mutability::Immutable,
                            memory_storage_type: decl.memory_storage_type.clone(),
                        },
                        value: DataValueType::None,
                    })),
                });
            }
            InstructionType::PushI => {}
            InstructionType::PushC => {}
            InstructionType::PushS => {}
            InstructionType::PopD => {}
            InstructionType::PopS(pops) => {
                let mut fn_context_ref = function_context.borrow_mut();
                let data = fn_context_ref.local_stack.pop().unwrap();
                let slot = fn_context_ref.local_data.get(pops.slot_id).unwrap();
                slot.value.replace(data.borrow().clone());
            }
            InstructionType::Add => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_add_values(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(result)));
            }
            InstructionType::Sub => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_subtract_values(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(result)));
            }
            InstructionType::Mul => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_multiply_values(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(result)));
            }
            InstructionType::Div => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_divide_values(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(result)));
            }
            InstructionType::Mod => {}
            InstructionType::LogOr => {}
            InstructionType::LogAnd => {}
            InstructionType::LogNot => {}
            InstructionType::BitAnd => {}
            InstructionType::BitOr => {}
            InstructionType::BitNot => {}
            InstructionType::Inv => {}
            InstructionType::EqC => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_compare_equal(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(DataValue {
                    data_type: DataTypeEncoding {
                        type_id: 0x6,
                        is_array: false,
                        mutability: Mutability::Immutable,
                        memory_storage_type: MemoryStorageType::Value,
                    },
                    value: DataValueType::Bool(result),
                })));
            }
            InstructionType::EqR => {}
            InstructionType::CLg => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_compare_greater(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(DataValue {
                    data_type: DataTypeEncoding {
                        type_id: 0x6,
                        is_array: false,
                        mutability: Mutability::Immutable,
                        memory_storage_type: MemoryStorageType::Value,
                    },
                    value: DataValueType::Bool(result),
                })));
            }
            InstructionType::CLgE => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_compare_greater_or_equal(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(DataValue {
                    data_type: DataTypeEncoding {
                        type_id: 0x6,
                        is_array: false,
                        mutability: Mutability::Immutable,
                        memory_storage_type: MemoryStorageType::Value,
                    },
                    value: DataValueType::Bool(result),
                })));
            }
            InstructionType::CLs => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_compare_less(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(DataValue {
                    data_type: DataTypeEncoding {
                        type_id: 0x6,
                        is_array: false,
                        mutability: Mutability::Immutable,
                        memory_storage_type: MemoryStorageType::Value,
                    },
                    value: DataValueType::Bool(result),
                })));
            }
            InstructionType::CLsE => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_compare_less_or_equal(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(DataValue {
                    data_type: DataTypeEncoding {
                        type_id: 0x6,
                        is_array: false,
                        mutability: Mutability::Immutable,
                        memory_storage_type: MemoryStorageType::Value,
                    },
                    value: DataValueType::Bool(result),
                })));
            }
            InstructionType::Invoke(call) => {
                let call_result = execute_function(call.function_id, context.clone());
                match call_result {
                    FunctionExecutionResult::Success(ret) => {
                        function_context
                            .borrow_mut()
                            .local_stack
                            .push(ret.clone());
                    }
                    FunctionExecutionResult::Failure(x) => {
                        panic!("Function execution failed: {}", x.fault_message)
                    }
                    FunctionExecutionResult::Invalid => {
                        panic!("Invalid function execution result.")
                    }
                }
            }
            InstructionType::Ret(ret) => {
                if ret.with_value {
                    let data = function_context
                        .borrow_mut()
                        .local_stack
                        .pop()
                        .unwrap();
                    return FunctionExecutionResult::Success(data);
                }
            }
            InstructionType::Throw => {}
            InstructionType::BTC => {}
            InstructionType::BT => {}
            InstructionType::BC => {}
            InstructionType::BF => {}
            InstructionType::ETC => {}
            InstructionType::Jmp(jump) => {
                if jump.jump_offset >= 0 {
                    let target_instruction = instruction_slice
                        .iter()
                        .filter(|i| i.offset > instruction.offset)
                        .filter(|i| match i.instruction_type {
                            InstructionType::Lbl => true,
                            _ => false,
                        })
                        .take(jump.jump_offset as usize)
                        .last()
                        .unwrap();

                    instruction_offset = target_instruction.offset;
                }
                else {
                    let target_instruction = instruction_slice
                        .iter()
                        .filter(|i| i.offset < instruction.offset)
                        .filter(|i| match i.instruction_type {
                            InstructionType::Lbl => true,
                            _ => false,
                        })
                        .rev()
                        .take(jump.jump_offset.abs() as usize)
                        .last()
                        .unwrap();

                    instruction_offset = target_instruction.offset;
                }

                continue;
            }
            InstructionType::JmpC(jump) => {
                let mut fn_context_ref = function_context.borrow_mut();
                let condition_data_value_ref = &fn_context_ref
                    .local_stack
                    .pop()
                    .unwrap();
                let condition_data_value = &condition_data_value_ref
                    .borrow()
                    .value;

                let condition = match condition_data_value {
                    DataValueType::Bool(b) => *b,
                    _ => panic!("Invalid data type for condition"),
                };

                if !condition {
                    if jump.jump_offset >= 0 {
                        let target_instruction = instruction_slice
                            .iter()
                            .filter(|i| i.offset > instruction.offset)
                            .filter(|i| match i.instruction_type {
                                InstructionType::Lbl => true,
                                _ => false,
                            })
                            .take(jump.jump_offset as usize)
                            .last()
                            .unwrap();

                        instruction_offset = target_instruction.offset;
                    }
                    else {
                        let target_instruction = instruction_slice
                            .iter()
                            .filter(|i| i.offset < instruction.offset)
                            .filter(|i| match i.instruction_type {
                                InstructionType::Lbl => true,
                                _ => false,
                            })
                            .rev()
                            .take(jump.jump_offset.abs() as usize)
                            .last()
                            .unwrap();

                        instruction_offset = target_instruction.offset;
                    }

                    continue;
                }
            }
            InstructionType::GType => {}
            InstructionType::WAll => {}
            InstructionType::TEvt => {}
            InstructionType::WEvt => {}
            InstructionType::CRt => {}
            InstructionType::CIR => {}
            InstructionType::DIR => {}
            InstructionType::Cln => {}
            InstructionType::TermP => {}
            InstructionType::TermEf => {}
            InstructionType::SEf => {}
            InstructionType::PEfId => {}
            InstructionType::CEfId => {}
            InstructionType::CType => {}
            InstructionType::ShL => {}
            InstructionType::ShR => {}
            InstructionType::Lbl => {
                trace!("Found label");
            }
            InstructionType::BitXor => {}
            InstructionType::FRet(_) => {}
            InstructionType::FCall(call) => {
                let call_result = execute_function(call.function_id, context.clone());
                match call_result {
                    FunctionExecutionResult::Success(_) => {}
                    FunctionExecutionResult::Failure(x) => {
                        panic!("Function execution failed: {}", x.fault_message);
                    }
                    FunctionExecutionResult::Invalid => {
                        panic!("Invalid function execution result.");
                    }
                }
            }
            InstructionType::LdStk(lsi) => match lsi.source {
                DataSourceType::ConstantTable => {
                    let data =
                        data::get_data_from_constant_table(context.clone(), lsi.location_id, true);
                    function_context
                        .borrow_mut()
                        .local_stack
                        .push(Rc::new(RefCell::new(data)));
                }
                DataSourceType::DataSlot => {
                    let data = data::get_data_from_data_slot(
                        function_context.clone(),
                        lsi.location_id,
                    );
                    function_context
                        .borrow_mut()
                        .local_stack
                        .push(data);
                }
                DataSourceType::DataHandle => {}
            },
            InstructionType::SvStk => {},
            InstructionType::NeqC => {
                let mut fn_context_ref = function_context.borrow_mut();
                let a = fn_context_ref.local_stack.pop().unwrap();
                let b = fn_context_ref.local_stack.pop().unwrap();

                let result = math::math_compare_not_equal(&b.borrow(), &a.borrow());
                fn_context_ref.local_stack.push(Rc::new(RefCell::new(DataValue {
                    data_type: DataTypeEncoding {
                        type_id: 0x6,
                        is_array: false,
                        mutability: Mutability::Immutable,
                        memory_storage_type: MemoryStorageType::Value,
                    },
                    value: DataValueType::Bool(result),
                })));
            },
            InstructionType::NeqR => {},
        }

        instruction_offset = next_instruction.offset;
    }

    let mut context_ref = context.borrow_mut();
    context_ref.stack_frames.pop_back();
    FunctionExecutionResult::Success(Rc::new(RefCell::new(DataValue {
        data_type: DataTypeEncoding {
            type_id: 0,
            is_array: false,
            mutability: Mutability::Immutable,
            memory_storage_type: MemoryStorageType::Reference,
        },
        value: DataValueType::None,
    })))
}
