use shared::models::package::Package;

pub fn decode_instructions(stream: &[u8], package: &Package) {
    println!("Instructions: {:02X?}", stream);
}
