use proc_macro::TokenStream;
use quote::quote;
use std::collections::HashMap;
use syn::{parse_macro_input, ImplItem, ItemImpl, LitStr};

#[proc_macro_attribute]
pub fn arc_scope_dispatcher(attr: TokenStream, item: TokenStream) -> TokenStream {
    // Get the scope name from the attribute
    let scope = parse_macro_input!(attr as LitStr).value();

    // Parse the item as an ItemImpl
    let mut input = parse_macro_input!(item as ItemImpl);
    let impl_type = &input.self_ty;

    // Get functions with arc_function_id attribute
    let mut function_mappings =HashMap::new();

    for item in &mut input.items {
        if let ImplItem::Fn(method) = item {
            for attr in &method.attrs {
                if attr.path().is_ident("arc_function_id") {
                    if let Ok(lit) = attr.parse_args::<syn::LitInt>() {
                        let function_id = lit.base10_parse::<usize>().unwrap();
                        let function_name = &method.sig.ident;
                        function_mappings.insert(function_id, function_name.clone());
                    }
                }
            }
        }
    }

    let dispatches = function_mappings.iter()
        .map(|(id, name)| {
            quote! { #id => Self::#name(args) }
        })
        .collect::<Vec<_>>();

    let expanded = quote! {
        impl shared::traits::scope_functions::ScopeFunctionDispatcher for #impl_type {
            fn dispatch_scope_functions(
                &self,
                function_id: usize,
                exec_context: std::rc::Rc<std::cell::RefCell<shared::models::execution::context::ExecutionContext>>,
            ) -> Result<shared::models::execution::result::FunctionExecutionResult, String> {
                let args = &mut exec_context.borrow_mut().global_stack;

                match function_id {
                    #(
                        #dispatches,
                    )*
                    _ => Err(format!("Unknown function 0x{:X} in {}", function_id, #scope)),
                }
            }
        }
        
        #input
    };

    TokenStream::from(expanded)
}

#[proc_macro_attribute]
pub fn arc_function_id(_attr: TokenStream, item: TokenStream) -> TokenStream {
    item
}
