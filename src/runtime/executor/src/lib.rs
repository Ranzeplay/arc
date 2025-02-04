use shared::models::descriptors::symbol::{Symbol, SymbolDescriptor};
use shared::models::execution::context::{ExecutionContext, FunctionExecutionContext};
use shared::models::execution::result::FunctionExecutionResult;
use shared::models::execution::result::FunctionExecutionResult::{Failure, Success};
use shared::models::package::Package;

pub fn launch(package: Package, verbose: bool) -> Result<i32, String> {
    println!("Launching program...");

    if verbose {
        println!("Package: {:?}", &package);
    }

    let mut context = ExecutionContext::new(package);
    let result = execute(&mut context);

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

pub fn execute(context: &mut ExecutionContext) -> FunctionExecutionResult {
    let entry_function = context
        .package
        .symbols
        .symbols
        .iter()
        .filter(|d| match d.value {
            Symbol::Function(_) => true,
            _ => false,
        })
        .collect::<Vec<SymbolDescriptor>>()
        .first()
        .unwrap();

    execute_function(entry_function.id, &mut context)
}

pub fn execute_function(id: usize, context: &mut ExecutionContext) -> FunctionExecutionResult {
    let entry_function = context
        .package
        .symbols
        .symbols
        .iter()
        .find(|d| d.id == id)
        .unwrap();

    let mut entry_function_context = FunctionExecutionContext {
        function: match entry_function {
            Symbol::Function(f) => f,
            _ => panic!("Invalid symbol type."),
        },
        local_data: Vec::new(),
    };

    context.stack_frames.push_back(entry_function_context);
    let entry_function_context = context.stack_frames.back_mut().unwrap();

    let entry_pos = entry_function_context.function.entry_pos;
    let block_length = entry_function_context.function.block_length;

    let instruction_pos = entry_pos;
    while instruction_pos < entry_pos + block_length {
        let instruction = &context.package.instructions.iter().find(|i| i.offset == instruction_pos).unwrap();
    }

    context.stack_frames.pop_back();
    FunctionExecutionResult::Invalid
}
