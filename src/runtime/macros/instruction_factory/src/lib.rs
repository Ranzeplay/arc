use lazy_static::lazy_static;
use proc_macro::TokenStream;
use quote::{format_ident, quote};
use std::collections::HashMap;
use std::sync::Mutex;
use syn::parse::{Parse, ParseStream};
use syn::Error;
use syn::Result;
use syn::{parse_macro_input, Expr, ExprLit, ItemImpl, Lit, Token, Type};

// Static storage for collected instructions
lazy_static! {
    // (op_code) -> (name, type_name)
    static ref INSTRUCTION_MAP: Mutex<HashMap<u8, (String, String)>> = Mutex::new(HashMap::new());
}

// Function to access the instruction map
fn get_instruction_map() -> HashMap<u8, (String, String)> {
    INSTRUCTION_MAP.lock().unwrap().clone()
}

// Custom parser for the attribute arguments
struct ArcInstructionArgs {
    op_code: u8,
    name: String,
}

impl Parse for ArcInstructionArgs {
    fn parse(input: ParseStream) -> Result<Self> {
        // Parse first argument (prefix)
        let op_code_expr: Expr = input.parse()?;

        // return Err(Error::new_spanned(&op_code_expr, format!("Parsing op_code_expr: {}", op_code_expr.to_token_stream())));

        let op_code = match op_code_expr {
            Expr::Lit(ExprLit { lit: Lit::Int(lit_int), .. }) => {
                lit_int.base10_parse::<u8>()?
            }
            // Handle cast expressions like 0x05u8
            Expr::Cast(cast_expr) => {
                match &*cast_expr.expr {
                    Expr::Lit(ExprLit { lit: Lit::Int(lit_int), .. }) => {
                        lit_int.base10_parse::<u8>()?
                    }
                    _ => return Err(Error::new_spanned(cast_expr, "Expected integer literal in cast expression")),
                }
            }
            Expr::Array(a) => {
                return Err(Error::new_spanned(a, "Array not supported for op_code"));
            }
            Expr::Assign(a) => {
                return Err(Error::new_spanned(a, "Assignment not supported for op_code"));
            }
            Expr::Macro(m) => {
                return Err(Error::new_spanned(m, "Macro not supported for op_code"));
            }
            Expr::Verbatim(v) => {
                return Err(Error::new_spanned(v, "Verbatim not supported for op_code"));
            },
            _ => return Err(Error::new_spanned(op_code_expr, "Unmatched")),
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

        Ok(ArcInstructionArgs { op_code, name })
    }
}

#[proc_macro_attribute]
pub fn arc_instruction(args: TokenStream, input: TokenStream) -> TokenStream {
    let input_clone = input.clone();

    let args = parse_macro_input!(args as ArcInstructionArgs);
    let impl_block = parse_macro_input!(input_clone as ItemImpl);

    let op_code = args.op_code;
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
        map.insert(op_code, (name.clone(), type_name.clone()));
    }

    input
}

#[proc_macro]
pub fn generate_instruction_enum(input: TokenStream) -> TokenStream {
    let enum_name = parse_macro_input!(input as syn::Ident);

    // Get the current instruction map
    let instruction_map = get_instruction_map();

    // Sort by opcode to ensure consistent ordering
    let mut instructions: Vec<_> = instruction_map.iter().collect();
    instructions.sort_by_key(|(op_code, _)| **op_code);

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

    let to_decode_sub_arms: Vec<_> = instructions
        .iter()
        .map(|(op_code, (op_name, type_name))| {
            let type_ident = format_ident!("{}", type_name);
            let op_ident = format_ident!("{}", op_name);
            quote! {
                #op_code => {
                    let result = #type_ident::decode(&stream, offset, package);

                    if let Some((inst, length)) = result {
                        return Some((#enum_name::#op_ident(inst), length));
                    } else {
                        return None;
                    }
                }
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

        impl #enum_name {
            pub fn decode(stream: &[u8], offset: usize, package: &Package) -> Option<(#enum_name, usize)> {
                let op_code = stream[0];

                match &op_code {
                    #(#to_decode_sub_arms,)*
                    _ => {}
                }

                return None;
            }
        }
    };

    output.into()
}

#[proc_macro]
pub fn arc_single_instruction(input: TokenStream) -> TokenStream {
    let args = parse_macro_input!(input as ArcInstructionArgs);
    let op_code = args.op_code;
    let name = format_ident!("{}", args.name);
    let struct_name = format_ident!("{}Instruction", name);
    let lit_name = args.name;

    let result = quote! {
        #[derive(PartialEq, Copy, Clone)]
        pub struct #struct_name {}

        impl std::fmt::Debug for #struct_name {
            fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
                write!(f, "")
            }
        }

        #[arc_instruction(#op_code, #lit_name)]
        impl DecodableInstruction<#struct_name> for #struct_name {
            fn decode(stream: &[u8], _offset: usize, _package: &Package) -> Option<(#struct_name, usize)> {
                let length = 1;

                Some((
                    #struct_name {},
                    length,
                ))
            }
        }
    };

    result.into()
}
