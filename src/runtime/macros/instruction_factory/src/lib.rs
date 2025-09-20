use proc_macro::TokenStream;
use quote::{format_ident, quote};
use syn::{parse_macro_input, Expr, ExprLit, ItemImpl, Lit, Token, Type};
use std::collections::HashMap;
use std::sync::Mutex;
use lazy_static::lazy_static;
use syn::parse::{Parse, ParseStream};
use syn::Result;
use syn::Error;

// Static storage for collected instructions
lazy_static! {
    static ref INSTRUCTION_MAP: Mutex<HashMap<u8, (String, String)>> = Mutex::new(HashMap::new());
}

// Function to access the instruction map
fn get_instruction_map() -> HashMap<u8, (String, String)> {
    INSTRUCTION_MAP.lock().unwrap().clone()
}

// Custom parser for the attribute arguments
struct ArcInstructionArgs {
    prefix: u8,
    name: String,
}

impl Parse for ArcInstructionArgs {
    fn parse(input: ParseStream) -> Result<Self> {
        // Parse first argument (prefix)
        let prefix_expr: Expr = input.parse()?;
        let prefix = match prefix_expr {
            Expr::Lit(ExprLit { lit: Lit::Int(lit_int), .. }) => {
                lit_int.base10_parse::<u8>()?
            }
            _ => return Err(Error::new_spanned(prefix_expr, "Expected u8 literal for prefix")),
        };

        // Parse comma
        input.parse::<Token![,]>()?;

        // Parse second argument (name)
        let name_expr: Expr = input.parse()?;
        let name = match name_expr {
            Expr::Lit(ExprLit { lit: Lit::Str(lit_str), .. }) => {
                lit_str.value()
            }
            _ => return Err(Error::new_spanned(name_expr, "Expected string literal for name")),
        };

        Ok(ArcInstructionArgs { prefix, name })
    }
}

#[proc_macro_attribute]
pub fn arc_instruction(args: TokenStream, input: TokenStream) -> TokenStream {
    let input_clone = input.clone();

    let args = parse_macro_input!(args as ArcInstructionArgs);
    let impl_block = parse_macro_input!(input_clone as ItemImpl);

    let prefix = args.prefix;
    let name = args.name;

    // Extract the type from the impl block
    let type_name = match &*impl_block.self_ty {
        Type::Path(type_path) => {
            quote! { #type_path }.to_string()
        }
        _ => panic!("Unsupported type in impl block"),
    };

    // Store in the global map
    {
        let mut map = INSTRUCTION_MAP.lock().unwrap();
        map.insert(prefix, (name.clone(), type_name.clone()));
    }

    input
}

#[proc_macro]
pub fn generate_instruction_enum(input: TokenStream) -> TokenStream {
    let enum_name = parse_macro_input!(input as syn::Ident);

    // Get the current instruction map
    let instruction_map = get_instruction_map();

    // Sort by prefix to ensure consistent ordering
    let mut instructions: Vec<_> = instruction_map.iter().collect();
    instructions.sort_by_key(|(prefix, _)| **prefix);

    // Generate enum variants with associated types
    let variants: Vec<_> = instructions
        .iter()
        .map(|(_, (name, type_name))| {
            let variant_name = format_ident!("{}", name);
            let type_ident = format_ident!("{}", type_name);

            quote! {
                #variant_name(#type_ident)
            }
        })
        .collect();

    let to_write_debug_sub_arms: Vec<_> = instructions
        .iter()
        .map(|(_, (name, _))| {
            let variant_name = format_ident!("{}", name);
            quote! {
                #enum_name::#variant_name(inst) => write!(f, "{:?}", inst)
            }
        })
        .collect();

    // Generate the complete enum with helper methods
    let output = quote! {
        #[derive(strum_macros::AsRefStr, PartialEq, Clone)]
        pub enum #enum_name {
            #(#variants,)*
        }

        impl std::fmt::Debug for #enum_name {
            fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
                match self {
                    #(#to_write_debug_sub_arms,)*
                }
            }
        }
    };

    output.into()
}
