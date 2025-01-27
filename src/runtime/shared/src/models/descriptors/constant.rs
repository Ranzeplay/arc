use std::fmt::Debug;

pub struct ConstantDescriptor {
    pub id: usize,
    pub type_id: usize,
    pub raw_value: Vec<u8>,
}

impl Debug for ConstantDescriptor {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "[I] 0x{:<016X}  ", self.id)?;
        write!(f, "[T] 0x{:<016X}  ", self.type_id)?;
        write!(f, "[L] {:02X?}", self.raw_value)
    }
}

pub struct ConstantTable {
    pub constants: Vec<ConstantDescriptor>,
}

impl Debug for ConstantTable {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "=== Constant table")?;
        for constant in &self.constants {
            writeln!(f, "{:?}", constant)?;
        }
        Ok(())
    }
}
