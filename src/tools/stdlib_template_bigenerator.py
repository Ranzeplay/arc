#!/usr/bin/env python3

sub_namespace = input("Enter subnamespace: ")

arc_side_text = f"""
link Arc::Std::Compilation;

namespace Arc::Std::{sub_namespace} {{
"""
runtime_side_text = f"""
use arc_shared::models::execution::data::DataValue;
use arc_shared::models::execution::result::FunctionExecutionResult;
use std::cell::RefCell;
use std::rc::Rc;
use arc_manual_function_id::arc_function_id;
use arc_scope_dispatcher::arc_scope_dispatcher;

pub struct ArcStd{sub_namespace} {{}};

#[arc_scope_dispatcher(\"Arc::Std::{sub_namespace}\")]
impl ArcStd{sub_namespace} {{

"""

while True:
	print("Define a new function, press enter directly to end:")
	func_name = input("Enter function name: ")
	if not func_name:
		break
	func_id = input("Enter function ID: ")
	func_params = input("Enter function parameter list: ")
	func_return = input("Enter function return type: ")

	arc_side_text += f"""
\t@DeclaratorOnly
\t@SymbolId({func_id})
\tpublic func {func_name}({func_params}): {func_return} {{}}
\n
	"""

	runtime_side_text += f"""
\t#[arc_function_id({func_id})]
\tpub fn {func_name}(args: &mut Vec<Rc<RefCell<DataValue>>>) -> Result<FunctionExecutionResult, String> {{
\t\tunimplemented!();
\t}}
\n
"""

# Finalize Arc stdlib bindings
arc_side_text += "}\n"

# Finalize runtime side bindings
runtime_side_text += "}\n"

# Write to files
with open(f"../compiler/Libraries/PackageGenerator/StdlibSource/{sub_namespace}.script.arc", "x") as arc_file:
	arc_file.write(arc_side_text[1:])


with open(f"../runtime/stdlib/src/{sub_namespace.lower()}.rs", "x") as rs_file:
	rs_file.write(runtime_side_text[1:])

print("Files generated successfully.")

# Display next steps
print(f"""
Next steps:
- Fix lib.rs usage
	Add `mod {sub_namespace.lower()};` to `lib.rs` in `runtime/stdlib`
- Adjust compiler/pkg_gen project settings to include the new file and register it in ArcStdlibLoader
	- Add `<Resource Include="StdlibSource\{sub_namespace}.script.arc" />` to the .csproj file in an item group with other stdlib files
	- Register the new stdlib file in `ArcStdlibLoader.cs` using the following:
		```
		var collectionNamespaceSource = Encoding.UTF8.GetString(ArcStdlibSource.{sub_namespace}Collection);
		var collectionNamespaceUnitContext = AntlrAdapter.ParseCompilationUnit({sub_namespace.lower()}NamespaceSource, logger);
		var collectionNamespaceUnit = new ArcCompilationUnit(collectionNamespaceUnitContext, logger, "Arc::Std::{sub_namespace}");
		```
- Implement the functions in the generated Rust file
	Edit `runtime/stdlib/src/{sub_namespace.lower()}.rs` and implement the functions you defined.
""")
