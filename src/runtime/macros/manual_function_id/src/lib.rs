use proc_macro::TokenStream;

#[proc_macro_attribute]
pub fn arc_function_id(_attr: TokenStream, item: TokenStream) -> TokenStream {
    item
}
