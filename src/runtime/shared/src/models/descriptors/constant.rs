use std::fmt::Debug;

pub struct ConstantDescriptor {
    pub id: usize,
    pub type_id: usize,
    pub raw_value: Vec<u8>,
}

impl Debug for ConstantDescriptor {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "[I]{:<20} [T]{:<20} [L]{:<20}", self.id, self.type_id, self.raw_value.len())
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
