use crate::models::package::Package;

pub trait DecodableInstruction<T> {
    fn decode(stream: &[u8], offset: usize, package: &Package) -> Option<(T, usize)>;
}
