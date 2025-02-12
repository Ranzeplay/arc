mod data;
mod func_exec;
mod instructions;
mod math;
mod stdlib;

use crate::func_exec::prepare_and_get_function_info;
use crate::instructions::function_call::execute_function_call;
use crate::instructions::return_function::wrap_return_value_if_needed;
use crate::stdlib::execute_stdlib_function;
use log::{debug, trace};
use shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::data::{DataSlot, DataValue, DataValueType};
use shared::models::execution::result::FunctionExecutionResult;
use shared::models::instruction::InstructionType;
use shared::models::instructions::stack_data_operation::{DataSourceType, LoadStackInstruction};
use shared::models::package::Package;
use std::cell::RefCell;
use std::rc::Rc;
use shared::models::instructions::decl::DeclInstruction;
use shared::models::instructions::pop_to_slot::PopToSlotInstruction;

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
    ($exec_context:expr, $op_func:expr) => {
        let mut exec_context_ref = $exec_context.borrow_mut();
        let a = exec_context_ref.global_stack.pop().unwrap();
        let b = exec_context_ref.global_stack.pop().unwrap();

        let result = $op_func(&b.borrow(), &a.borrow());
        exec_context_ref
            .global_stack
            .push(Rc::new(RefCell::new(result)));
    };
}

macro_rules! comparison_operation_instruction {
    ($fn_context:expr, $compare_func:expr) => {
        let mut exec_context_ref = $fn_context.borrow_mut();
        let a = exec_context_ref.global_stack.pop().unwrap();
        let b = exec_context_ref.global_stack.pop().unwrap();

        let result = $compare_func(&b.borrow(), &a.borrow());
        push_bool_to_stack!(exec_context_ref.global_stack, result);
    };
}

pub fn launch(package: Package, _verbose: bool) -> Result<i32, String> {
    debug!("Launching program...");

    let result = execute(package);

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

pub fn execute(package: Package) -> FunctionExecutionResult {
    let mut context = ExecutionContext::new(package);
    context.init_jump_destinations();
    context.init_function_entry_points();

    execute_function(
        context.package.descriptor.entrypoint_function_id,
        None,
        Rc::new(RefCell::new(context)),
    )
}

pub fn execute_function(
    function_id: usize,
    parent_fn_opt: Option<Rc<RefCell<FunctionExecutionContext>>>,
    exec_context: Rc<RefCell<ExecutionContext>>,
) -> FunctionExecutionResult {
    let mut result = FunctionExecutionResult::Invalid;

    if function_id >= 0xa1 && function_id <= 0xff {
        trace!("Executing stdlib function");
        execute_stdlib_function(function_id, Rc::clone(&exec_context));
        return FunctionExecutionResult::Success(None);
    }

    let func_info = prepare_and_get_function_info(function_id, parent_fn_opt, Rc::clone(&exec_context));
    let function_context = Rc::clone(&func_info.function_context);

    let mut instruction_index = {
        let exec_context_ref = exec_context.borrow();
        let loc = exec_context_ref.function_entry_points[&function_id];

        loc
    };

    loop {
        let instruction = {
            let exec_context_ref = exec_context.borrow();
            let result = exec_context_ref.package.instructions.get(instruction_index).unwrap();

            Rc::clone(&result)
        };

        match &instruction.instruction_type {
            InstructionType::Decl(decl) => {
                declare_data(&function_context, decl);
            }
            InstructionType::PushI => {}
            InstructionType::PushC => {}
            InstructionType::PushS => {}
            InstructionType::PopD => {}
            InstructionType::PopS(pops) => {
                pop_to_slot(&exec_context, &function_context, pops);
            }
            InstructionType::Add => {
                arithmetic_operation_instruction!(exec_context, math::math_add_values);
            }
            InstructionType::Sub => {
                arithmetic_operation_instruction!(exec_context, math::math_subtract_values);
            }
            InstructionType::Mul => {
                arithmetic_operation_instruction!(exec_context, math::math_multiply_values);
            }
            InstructionType::Div => {
                arithmetic_operation_instruction!(exec_context, math::math_divide_values);
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
                comparison_operation_instruction!(exec_context, math::math_compare_equal);
            }
            InstructionType::EqR => {}
            InstructionType::CLg => {
                comparison_operation_instruction!(exec_context, math::math_compare_greater);
            }
            InstructionType::CLgE => {
                comparison_operation_instruction!(
                    exec_context,
                    math::math_compare_greater_or_equal
                );
            }
            InstructionType::CLs => {
                comparison_operation_instruction!(exec_context, math::math_compare_less);
            }
            InstructionType::CLsE => {
                comparison_operation_instruction!(exec_context, math::math_compare_less_or_equal);
            }
            InstructionType::NeqC => {
                comparison_operation_instruction!(exec_context, math::math_compare_not_equal);
            }
            InstructionType::NeqR => {}
            InstructionType::Invoke(call) => {
                execute_function_call(
                    Rc::clone(&exec_context),
                    Rc::clone(&function_context),
                    call.function_id,
                );
            }
            InstructionType::Ret(ret) => {
                result = wrap_return_value_if_needed(Rc::clone(&exec_context), ret.with_value);
                break;
            }
            InstructionType::Throw => {}
            InstructionType::BTC => {}
            InstructionType::BT => {}
            InstructionType::BC => {}
            InstructionType::BF => {}
            InstructionType::ETC => {}
            InstructionType::Jmp(_) => {
                instruction_index = exec_context.borrow().jump_destinations[&instruction_index];
                continue;
            }
            InstructionType::JmpC(_) => {
                let condition = {
                    let mut exec_context_ref = exec_context.borrow_mut();
                    let condition_data = exec_context_ref.global_stack.pop().unwrap();

                    let c = match condition_data.borrow().value {
                        DataValueType::Bool(b) => b,
                        _ => panic!("Invalid data type for condition"),
                    };

                    c
                };

                if !condition {
                    instruction_index = exec_context.borrow().jump_destinations[&instruction_index];
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
                result = wrap_return_value_if_needed(exec_context, ret.with_value);
                break;
            }
            InstructionType::FCall(call) => {
                execute_function_call(
                    Rc::clone(&exec_context),
                    Rc::clone(&function_context),
                    call.function_id,
                );
            }
            InstructionType::LdStk(lsi) => load_stack(&exec_context, Rc::clone(&function_context), lsi),
            InstructionType::SvStk(_ssi) => {}
        }

        instruction_index += 1;
    }

    match result {
        FunctionExecutionResult::Invalid => FunctionExecutionResult::Success(None),
        _ => result,
    }
}

fn declare_data(function_context: &Rc<RefCell<FunctionExecutionContext>>, decl: &DeclInstruction) {
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

fn pop_to_slot(exec_context: &Rc<RefCell<ExecutionContext>>, function_context: &Rc<RefCell<FunctionExecutionContext>>, pops: &PopToSlotInstruction) {
    let fn_context_ref = function_context.borrow();
    let mut exec_context_ref = exec_context.borrow_mut();
    let data = exec_context_ref.global_stack.pop().unwrap();
    let slot = fn_context_ref.local_data.get(pops.slot_id).unwrap();
    slot.value.replace(data.borrow().to_owned());
}

fn load_stack(exec_context: &Rc<RefCell<ExecutionContext>>, function_context: Rc<RefCell<FunctionExecutionContext>>, lsi: &LoadStackInstruction) {
    match lsi.source {
        DataSourceType::ConstantTable => {
            let data = data::get_data_from_constant_table(
                &exec_context.borrow().package,
                lsi.location_id,
                true,
            );
            exec_context
                .borrow_mut()
                .global_stack
                .push(Rc::new(RefCell::new(data)));
        }
        DataSourceType::DataSlot => {
            let data =
                data::get_data_from_data_slot(function_context.clone(), lsi.location_id);
            exec_context.borrow_mut().global_stack.push(data);
        }
        DataSourceType::DataHandle => {}
    }
}
