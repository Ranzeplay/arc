mod data;
mod func_exec;
mod instructions;
mod math;
mod stdlib;

use crate::func_exec::{end_function, prepare_and_get_function_info};
use crate::instructions::function_call::execute_function_call;
use crate::instructions::jump::get_jump_destination;
use crate::instructions::return_function::wrap_return_value_if_needed;
use crate::stdlib::execute_stdlib_function;
use log::{debug, trace};
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::context::{ExecutionContext};
use shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use shared::models::execution::result::FunctionExecutionResult;
use shared::models::instruction::InstructionType;
use shared::models::instructions::stack_data_operation::DataSourceType;
use shared::models::package::Package;
use std::cell::RefCell;
use std::rc::Rc;

macro_rules! push_bool_to_stack {
    ($stack:expr, $result:expr) => {
        $stack.push(Rc::new(RefCell::new(DataValue {
            data_type: DataTypeEncoding {
                type_id: 0x6,
                is_array: false,
                mutability: Mutability::Immutable,
                memory_storage_type: MemoryStorageType::Value,
            },
            value: DataValueType::Bool($result),
        })));
    };
}

macro_rules! arithmetic_operation_instruction {
    ($fn_context:expr, $op_func:expr) => {
        let mut fn_context_ref = $fn_context.borrow_mut();
        let a = fn_context_ref.local_stack.pop().unwrap();
        let b = fn_context_ref.local_stack.pop().unwrap();

        let result = $op_func(&b.borrow(), &a.borrow());
        fn_context_ref
            .local_stack
            .push(Rc::new(RefCell::new(result)));
    };
}

macro_rules! comparison_operation_instruction {
    ($fn_context:expr, $compare_func:expr) => {
        let mut fn_context_ref = $fn_context.borrow_mut();
        let a = fn_context_ref.local_stack.pop().unwrap();
        let b = fn_context_ref.local_stack.pop().unwrap();

        let result = $compare_func(&b.borrow(), &a.borrow());
        push_bool_to_stack!(fn_context_ref.local_stack, result);
    };
}

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
    let mut result = FunctionExecutionResult::Invalid;

    if function_id >= 0xa1 && function_id <= 0xff {
        trace!("Executing stdlib function");
        execute_stdlib_function(function_id, &context);
        return FunctionExecutionResult::Success(None);
    }

    let func_info = prepare_and_get_function_info(context.clone(), function_id);
    let function_context = func_info.function_context.clone();
    let instruction_slice = func_info.instruction_slice.clone();

    let mut instruction_offset = func_info.entry_pos;
    loop {
        if instruction_offset >= func_info.entry_pos + func_info.block_length - 1 {
            break;
        }

        let instructions = func_info.instruction_slice
            .iter()
            .filter(|i| i.offset >= instruction_offset)
            .take(2)
            .collect::<Vec<_>>();

        if instructions.is_empty() {
            break;
        }

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
                arithmetic_operation_instruction!(function_context, math::math_add_values);
            }
            InstructionType::Sub => {
                arithmetic_operation_instruction!(function_context, math::math_subtract_values);
            }
            InstructionType::Mul => {
                arithmetic_operation_instruction!(function_context, math::math_multiply_values);
            }
            InstructionType::Div => {
                arithmetic_operation_instruction!(function_context, math::math_divide_values);
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
                comparison_operation_instruction!(function_context, math::math_compare_equal);
            }
            InstructionType::EqR => {}
            InstructionType::CLg => {
                comparison_operation_instruction!(function_context, math::math_compare_greater);
            }
            InstructionType::CLgE => {
                comparison_operation_instruction!(
                    function_context,
                    math::math_compare_greater_or_equal
                );
            }
            InstructionType::CLs => {
                comparison_operation_instruction!(function_context, math::math_compare_less);
            }
            InstructionType::CLsE => {
                comparison_operation_instruction!(
                    function_context,
                    math::math_compare_less_or_equal
                );
            }
            InstructionType::NeqC => {
                comparison_operation_instruction!(function_context, math::math_compare_not_equal);
            }
            InstructionType::NeqR => {}
            InstructionType::Invoke(call) => {
                execute_function_call(context.clone(), function_context.clone(), call.function_id);
            }
            InstructionType::Ret(ret) => {
                result = wrap_return_value_if_needed(&function_context, ret.with_value);
                break;
            }
            InstructionType::Throw => {}
            InstructionType::BTC => {}
            InstructionType::BT => {}
            InstructionType::BC => {}
            InstructionType::BF => {}
            InstructionType::ETC => {}
            InstructionType::Jmp(jump) => {
                instruction_offset =
                    get_jump_destination(&instruction_slice, instruction, jump.jump_offset);

                continue;
            }
            InstructionType::JmpC(jump) => {
                let mut fn_context_ref = function_context.borrow_mut();
                let condition_data_value_ref = &fn_context_ref.local_stack.pop().unwrap();
                let condition_data_value = &condition_data_value_ref.borrow().value;

                let condition = match condition_data_value {
                    DataValueType::Bool(b) => *b,
                    _ => panic!("Invalid data type for condition"),
                };

                if !condition {
                    instruction_offset =
                        get_jump_destination(&instruction_slice, instruction, jump.jump_offset);

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
            InstructionType::FRet(ret) => {
                result = wrap_return_value_if_needed(&function_context, ret.with_value);
                break;
            }
            InstructionType::FCall(call) => {
                execute_function_call(context.clone(), function_context.clone(), call.function_id);
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
                    let data =
                        data::get_data_from_data_slot(function_context.clone(), lsi.location_id);
                    function_context.borrow_mut().local_stack.push(data);
                }
                DataSourceType::DataHandle => {}
            },
            InstructionType::SvStk(ssi) => {}
        }

        instruction_offset = next_instruction.offset;
    }

    end_function(context.clone());

    match result {
        FunctionExecutionResult::Invalid => FunctionExecutionResult::Success(None),
        _ => result,
    }
}
