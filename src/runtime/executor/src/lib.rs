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
use arc_shared::models::encodings::data_type_enc::{DataTypeEncoding, MemoryStorageType, Mutability};
use arc_shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use arc_shared::models::execution::data::{DataValue, DataValueType};
use arc_shared::models::execution::result::FunctionExecutionResult;
use arc_shared::models::instruction::InstructionType;
use std::cell::RefCell;
use std::rc::Rc;
use arc_shared::models::options::launch_options::LaunchOptions;
use crate::instructions::data_declaration::{construct_data, declare_data};
use crate::instructions::stack_operations::{load_stack, pop_to_slot, replace_stack_top, save_stack};
use crate::math::{math_bitwise_not, math_logical_and, math_logical_not, math_logical_or};

macro_rules! push_bool_to_stack {
    ($stack:expr, $result:expr) => {
        $stack.push(Rc::new(RefCell::new(DataValue {
            data_type: DataTypeEncoding {
                type_id: 0x6,
                dimension: 0,
                mutability: Mutability::Immutable,
                memory_storage_type: MemoryStorageType::Value,
            },
            value: DataValueType::Bool($result),
        })));
    };
}

pub fn launch(opt: Rc<LaunchOptions>) -> Result<i32, String> {
    debug!("Launching program...");

    let result = execute(opt);

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

pub fn execute(opt: Rc<LaunchOptions>) -> FunctionExecutionResult {
    let mut context = ExecutionContext::new(Rc::clone(&opt.package), opt.args.clone());
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
    let result;

    if function_id >= 0xa1 && function_id <= 0xff {
        trace!("Executing stdlib function");
        execute_stdlib_function(function_id, Rc::clone(&exec_context));
        return FunctionExecutionResult::Success(None);
    }

    let function_context = prepare_and_get_function_info(function_id, parent_fn_opt, Rc::clone(&exec_context));

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
                declare_data(&function_context, decl, &exec_context.borrow().package);
            }
            InstructionType::PushI => {}
            InstructionType::PushC => {}
            InstructionType::PushS => {}
            InstructionType::PopD => {}
            InstructionType::PopS(pops) => {
                pop_to_slot(&exec_context, &function_context, pops);
            }
            InstructionType::Add => {
                binary_operation_instruction!(exec_context, math::math_add_values);
            }
            InstructionType::Sub => {
                binary_operation_instruction!(exec_context, math::math_subtract_values);
            }
            InstructionType::Mul => {
                binary_operation_instruction!(exec_context, math::math_multiply_values);
            }
            InstructionType::Div => {
                binary_operation_instruction!(exec_context, math::math_divide_values);
            }
            InstructionType::Mod => {
                binary_operation_instruction!(exec_context, math::math_modulo_values);
            }
            InstructionType::LogOr => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();
                let b = exec_context_ref.global_stack.pop().unwrap();

                let result = math_logical_or(&b.borrow(), &a.borrow());
                push_bool_to_stack!(exec_context_ref.global_stack, result);
            }
            InstructionType::LogAnd => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();
                let b = exec_context_ref.global_stack.pop().unwrap();

                let result = math_logical_and(&b.borrow(), &a.borrow());
                push_bool_to_stack!(exec_context_ref.global_stack, result);
            }
            InstructionType::LogNot => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();

                let result = math_logical_not(&a.borrow());
                push_bool_to_stack!(exec_context_ref.global_stack, result);
            }
            InstructionType::BitAnd => {
                binary_operation_instruction!(exec_context, math::math_bitwise_and);
            }
            InstructionType::BitOr => {
                binary_operation_instruction!(exec_context, math::math_bitwise_or);
            }
            InstructionType::BitNot => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();
                let result = math_bitwise_not(&a.borrow());

                exec_context_ref
                    .global_stack
                    .push(Rc::new(RefCell::new(result)));
            }
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
            InstructionType::ShL => {
                binary_operation_instruction!(exec_context, math::math_bitwise_left_shift);
            }
            InstructionType::ShR => {
                binary_operation_instruction!(exec_context, math::math_bitwise_right_shift);
            }
            InstructionType::Lbl => {
                trace!("Found label");
            }
            InstructionType::BitXor => {
                binary_operation_instruction!(exec_context, math::math_bitwise_xor);
            }
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
            InstructionType::SvStk(ssi) => save_stack(&exec_context, Rc::clone(&function_context), ssi),
            InstructionType::RpStk => replace_stack_top(&exec_context),
            InstructionType::NewObj(no) => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let obj = construct_data(no, &exec_context_ref.package);

                exec_context_ref.global_stack.push(obj);
            }
        }

        instruction_index += 1;
    }

    result
}
