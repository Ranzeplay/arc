use shared::models::descriptors::symbol::{Symbol, SymbolDescriptor};
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::result::FunctionExecutionResult;
use shared::models::execution::result::FunctionExecutionResult::{Failure, Success};
use shared::models::instruction::InstructionType;
use shared::models::package::Package;
use std::cell::RefCell;
use std::rc::Rc;

pub fn launch(package: Package, _verbose: bool) -> Result<i32, String> {
    println!("Launching program...");

    let context = ExecutionContext::new(package);
    let context_rc = Rc::new(RefCell::new(context));
    let result = execute(context_rc);

    match result {
        Success(_) => {
            println!("Program executed successfully.");
            Ok(0)
        }
        Failure(fault) => {
            println!("Program execution failed: {}", fault.fault_message);
            Ok(1)
        }
        FunctionExecutionResult::Invalid => Ok(0),
    }
}

pub fn execute(context: Rc<RefCell<ExecutionContext>>) -> FunctionExecutionResult {
    let context_ref = context.borrow();
    let binding = context_ref
        .package
        .symbols
        .symbols
        .iter()
        .filter(|&d| match d.value {
            Symbol::Function(_) => true,
            _ => false,
        })
        .collect::<Vec<&SymbolDescriptor>>();
    let entry_function = binding.first().unwrap();

    execute_function(entry_function.id, context.clone())
}

pub fn execute_function(
    function_id: usize,
    context: Rc<RefCell<ExecutionContext>>,
) -> FunctionExecutionResult {
    let context_ref = context.borrow();
    let mut context_mut_ref = context.borrow_mut();
    let entry_function = context_ref
        .package
        .symbols
        .symbols
        .iter()
        .find(|d| d.id == function_id)
        .unwrap();

    let entry_function_context = FunctionExecutionContext {
        function: match &entry_function.value {
            Symbol::Function(f) => f.clone(),
            _ => panic!("Invalid symbol type."),
        },
        local_data: Vec::new(),
    };

    context_mut_ref
        .stack_frames
        .push_back(entry_function_context);
    let entry_function_context = context_mut_ref.stack_frames.back_mut().unwrap();

    let entry_pos = entry_function_context.function.entry_pos;
    let block_length = entry_function_context.function.block_length;

    let instruction_slice = context_ref
        .package
        .instructions
        .iter()
        .take_while(|i| i.offset >= entry_pos)
        .collect::<Vec<_>>();

    for instruction in instruction_slice {
        if instruction.offset >= entry_pos + block_length {
            break;
        }

        match &instruction.instruction_type {
            InstructionType::Decl(_) => {}
            InstructionType::PushI => {}
            InstructionType::PushC => {}
            InstructionType::PushS => {}
            InstructionType::PopD => {}
            InstructionType::PopS(_) => {}
            InstructionType::Add => {}
            InstructionType::Sub => {}
            InstructionType::Mul => {}
            InstructionType::Div => {}
            InstructionType::Mod => {}
            InstructionType::LogOr => {}
            InstructionType::LogAnd => {}
            InstructionType::LogNot => {}
            InstructionType::BitAnd => {}
            InstructionType::BitOr => {}
            InstructionType::BitNot => {}
            InstructionType::Inv => {}
            InstructionType::EqC => {}
            InstructionType::EqR => {}
            InstructionType::CLg => {}
            InstructionType::CLgE => {}
            InstructionType::CLs => {}
            InstructionType::CLsE => {}
            InstructionType::Invoke(call) => {
                let call_result = execute_function(call.function_id, context.clone());
                match call_result {
                    Success(_) => {}
                    Failure(x) => panic!("Function execution failed: {}", x.fault_message),
                    FunctionExecutionResult::Invalid => {
                        panic!("Invalid function execution result.")
                    }
                }
            }
            InstructionType::Ret(_) => {}
            InstructionType::Throw => {}
            InstructionType::BTC => {}
            InstructionType::BT => {}
            InstructionType::BC => {}
            InstructionType::BF => {}
            InstructionType::ETC => {}
            InstructionType::Jmp(_) => {}
            InstructionType::JmpC(_) => {}
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
            InstructionType::Lbl => {}
            InstructionType::BitXor => {}
            InstructionType::FRet(_) => {}
            InstructionType::FCall(call) => {
                let call_result = execute_function(call.function_id, context.clone());
                match call_result {
                    Success(_) => {}
                    Failure(x) => panic!("Function execution failed: {}", x.fault_message),
                    FunctionExecutionResult::Invalid => {
                        panic!("Invalid function execution result.")
                    }
                }
            }
            InstructionType::LdStk(_) => {}
            InstructionType::SvStk => {}
        }
    }

    context_mut_ref.stack_frames.pop_back();
    FunctionExecutionResult::Invalid
}
