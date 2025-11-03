mod data;
mod func_exec;
mod instructions;
mod math;
mod stdlib;

use crate::func_exec::prepare_and_get_function_info;
use crate::instructions::return_function::wrap_return_value_if_needed;
use crate::stdlib::execute_stdlib_function;
use log::{debug, error, trace, warn};
use arc_shared::models::encodings::data_type_enc::{DataTypeEncoding, Mutability};
use arc_shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use arc_shared::models::execution::data::{DataValue, DataValueType};
use arc_shared::models::execution::result::{FunctionExecutionFault, FunctionExecutionResult, StackTraceLocation};
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
                mutability: Mutability::Immutable
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
            Ok(0x00)
        }
        FunctionExecutionResult::Failure(fault) => {
            error!("Program execution failed.");
            error!("{}", fault.borrow());
            Ok(0xff)
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
    trace!("Entering function 0x{:016X}", function_id);

    let result;

    if function_id >= 0xa1 && function_id <= 0xff {
        trace!("Executing stdlib function");
        execute_stdlib_function(function_id, Rc::clone(&exec_context));
        trace!("Exiting function 0x{:016X}", function_id);
        return FunctionExecutionResult::Success(None);
    }

    let function_context = prepare_and_get_function_info(function_id, parent_fn_opt, Rc::clone(&exec_context));

    let mut instruction_index = {
        let exec_context_ref = exec_context.borrow();
        let loc = exec_context_ref.function_entry_points[&function_id];

        loc
    };

    'finalize: loop {
        let instruction = {
            let exec_context_ref = exec_context.borrow();
            let result = exec_context_ref.package.instructions.get(instruction_index).unwrap();

            Rc::clone(&result)
        };

        trace!("  {:?}", instruction);

        match &instruction.instruction_type {
            InstructionType::Decl(decl) => {
                declare_data(&function_context, decl, &exec_context.borrow().package);
            }
            InstructionType::PushI(_) => {}
            InstructionType::PushC(_) => {}
            InstructionType::PushS(_) => {}
            InstructionType::PopD(_) => {}
            InstructionType::PopS(pops) => {
                pop_to_slot(&exec_context, &function_context, pops);
            }
            InstructionType::Add(_) => {
                binary_operation_instruction!(exec_context, math::math_add_values);
            }
            InstructionType::Sub(_) => {
                binary_operation_instruction!(exec_context, math::math_subtract_values);
            }
            InstructionType::Mul(_) => {
                binary_operation_instruction!(exec_context, math::math_multiply_values);
            }
            InstructionType::Div(_) => {
                binary_operation_instruction!(exec_context, math::math_divide_values);
            }
            InstructionType::Mod(_) => {
                binary_operation_instruction!(exec_context, math::math_modulo_values);
            }
            InstructionType::LogOr(_) => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();
                let b = exec_context_ref.global_stack.pop().unwrap();

                let result = math_logical_or(&b.borrow(), &a.borrow());
                push_bool_to_stack!(exec_context_ref.global_stack, result);
            }
            InstructionType::LogAnd(_) => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();
                let b = exec_context_ref.global_stack.pop().unwrap();

                let result = math_logical_and(&b.borrow(), &a.borrow());
                push_bool_to_stack!(exec_context_ref.global_stack, result);
            }
            InstructionType::LogNot(_) => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();

                let result = math_logical_not(&a.borrow());
                push_bool_to_stack!(exec_context_ref.global_stack, result);
            }
            InstructionType::BitAnd(_) => {
                binary_operation_instruction!(exec_context, math::math_bitwise_and);
            }
            InstructionType::BitOr(_) => {
                binary_operation_instruction!(exec_context, math::math_bitwise_or);
            }
            InstructionType::BitNot(_) => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let a = exec_context_ref.global_stack.pop().unwrap();
                let result = math_bitwise_not(&a.borrow());

                exec_context_ref
                    .global_stack
                    .push(Rc::new(RefCell::new(result)));
            }
            InstructionType::Inv(_) => {}
            InstructionType::EqC(_) => {
                comparison_operation_instruction!(exec_context, math::math_compare_equal);
            }
            InstructionType::EqR(_) => {}
            InstructionType::CLg(_) => {
                comparison_operation_instruction!(exec_context, math::math_compare_greater);
            }
            InstructionType::CLgE(_) => {
                comparison_operation_instruction!(
                    exec_context,
                    math::math_compare_greater_or_equal
                );
            }
            InstructionType::CLs(_) => {
                comparison_operation_instruction!(exec_context, math::math_compare_less);
            }
            InstructionType::CLsE(_) => {
                comparison_operation_instruction!(exec_context, math::math_compare_less_or_equal);
            }
            InstructionType::NeqC(_) => {
                comparison_operation_instruction!(exec_context, math::math_compare_not_equal);
            }
            InstructionType::NeqR(_) => {}
            InstructionType::Ret(ret) => {
                result = wrap_return_value_if_needed(Rc::clone(&exec_context), ret.with_value);
                break;
            }
            InstructionType::Throw(_) => {
                let mut exec_context_ref = exec_context.borrow_mut();
                let exception = exec_context_ref.global_stack.pop().unwrap();

                return FunctionExecutionResult::Failure(Rc::new(RefCell::new(
                    FunctionExecutionFault {
                        stack_trace: vec![StackTraceLocation::new(instruction_index, function_id)],
                        exception: Rc::clone(&exception),
                    },
                )));
            }
            InstructionType::BTC(_) => {}
            InstructionType::BT(_) => {}
            InstructionType::BC(_) => {}
            InstructionType::BF(_) => {}
            InstructionType::ETC(_) => {}
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
            InstructionType::GType(_) => {}
            InstructionType::WAll(_) => {}
            InstructionType::TEvt(_) => {}
            InstructionType::WEvt(_) => {}
            InstructionType::CRt(_) => {}
            InstructionType::CIR(_) => {}
            InstructionType::DIR(_) => {}
            InstructionType::Cln(_) => {}
            InstructionType::TermP(_) => {}
            InstructionType::TermEf(_) => {}
            InstructionType::SEf(_) => {}
            InstructionType::PEfId(_) => {}
            InstructionType::CEfId(_) => {}
            InstructionType::CType(_) => {}
            InstructionType::ShL(_) => {
                binary_operation_instruction!(exec_context, math::math_bitwise_left_shift);
            }
            InstructionType::ShR(_) => {
                binary_operation_instruction!(exec_context, math::math_bitwise_right_shift);
            }
            InstructionType::Lbl(_) => {
                trace!("Found label");
            }
            InstructionType::BitXor(_) => {
                binary_operation_instruction!(exec_context, math::math_bitwise_xor);
            }
            InstructionType::FRet(ret) => {
                result = wrap_return_value_if_needed(exec_context, ret.with_value);
                break 'finalize;
            }
            InstructionType::FCall(call) => {
                let mut fn_id = call.function_id;
                if fn_id == 0 {
                    fn_id = {
                        let mut exec_context_ref = exec_context.borrow_mut();
                        let symbol_data = exec_context_ref.global_stack.pop().unwrap();
                        let symbol_data = symbol_data.borrow();

                        match &symbol_data.value {
                            DataValueType::Symbol(s) => s.id,
                            _ => {
                                error!("Invalid data type for function symbol");
                                return FunctionExecutionResult::Invalid;
                            }
                        }
                    };
                }

                let fn_result = execute_function(fn_id, Some(Rc::clone(&function_context)), Rc::clone(&exec_context));
                match fn_result {
                    FunctionExecutionResult::Invalid => {
                        error!("Invalid function(0x{:016X}) execution result", call.function_id);
                        return FunctionExecutionResult::Invalid;
                    }
                    FunctionExecutionResult::Success(data_opt) => {
                        if let Some(data) = data_opt {
                            exec_context.borrow_mut().global_stack.push(data);
                        }
                    }
                    FunctionExecutionResult::Failure(exception) => {
                        {
                            let mut exception_ref = exception.borrow_mut();
                            exception_ref.stack_trace.push(StackTraceLocation::new(instruction_index, function_id));
                        }

                        return FunctionExecutionResult::Failure(exception);
                    }
                }
            }
            InstructionType::LdStk(lsi) => load_stack(&exec_context, Rc::clone(&function_context), lsi),
            InstructionType::SvStk(ssi) => save_stack(&exec_context, Rc::clone(&function_context), ssi),
            InstructionType::RpStk(_) => replace_stack_top(&exec_context),
            InstructionType::NewObj(no) => {
                {
                    let mut exec_context_ref = exec_context.borrow_mut();
                    let obj = construct_data(no, &exec_context_ref.package);

                    exec_context_ref.global_stack.push(obj);
                }

                let ctor_result = execute_function(no.ctor_fn_id, Some(Rc::clone(&function_context)), Rc::clone(&exec_context));
                match ctor_result {
                    FunctionExecutionResult::Invalid => {
                        error!("Invalid constructor function(0x{:016X}) execution result", no.ctor_fn_id);
                        return FunctionExecutionResult::Invalid;
                    }
                    FunctionExecutionResult::Success(data_opt) => {
                        if let Some(data) = data_opt {
                            exec_context.borrow_mut().global_stack.push(data);
                        }
                    }
                    FunctionExecutionResult::Failure(exception) => {
                        {
                            let mut exception_ref = exception.borrow_mut();
                            exception_ref.stack_trace.push(StackTraceLocation::new(instruction_index, function_id));
                        }

                        return FunctionExecutionResult::Failure(exception);
                    }
                }
            }
        }

        instruction_index += 1;
    }

    {
        match &result {
            FunctionExecutionResult::Invalid => {}
            FunctionExecutionResult::Success(s) => {
                match s {
                    None => trace!("Exiting function 0x{:016X} with no value", function_id),
                    Some(v) => trace!("Exiting function 0x{:016X} with value: {:?}", function_id, v.borrow().value),
                }
            }
            FunctionExecutionResult::Failure(f) => trace!("Exiting function 0x{:016X} with failure: {:?}", function_id, f.borrow())
        }
    }


    result
}
