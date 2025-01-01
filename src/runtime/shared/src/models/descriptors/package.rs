use std::fmt::Debug;

pub enum PackageType {
    Library,
    Executable,
    Invalid
}

pub struct PackageDescriptor {
    pub package_type: PackageType,
    pub name: String,
    pub version: usize,
    pub entrypoint_function_id: usize,
    pub data_alignment_length: usize,
    pub root_function_table_entry_position: usize,
    pub root_constant_table_entry_position: usize,
    pub root_group_table_entry_position: usize,
    pub region_table_entry_position: usize,
}

impl PackageDescriptor {
    pub fn new() -> PackageDescriptor {
        PackageDescriptor {
            package_type: PackageType::Invalid,
            name: String::new(),
            version: 0,
            entrypoint_function_id: 0,
            data_alignment_length: 0,
            root_function_table_entry_position: 0,
            root_constant_table_entry_position: 0,
            root_group_table_entry_position: 0,
            region_table_entry_position: 0,
        }
    }
}

impl Debug for PackageDescriptor {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "=== Package descriptor")?;

        writeln!(f, "Package Type:                          {}", match self.package_type {
            PackageType::Library => "Library",
            PackageType::Executable => "Executable",
            PackageType::Invalid => "Invalid",
        })?;

        writeln!(f, "Name:                                  {}", self.name)?;
        writeln!(f, "Version:                               {}", self.version)?;
        writeln!(f, "Entrypoint Function ID:                {}", self.entrypoint_function_id)?;
        writeln!(f, "Data Alignment Length:                 {}", self.data_alignment_length)?;
        writeln!(f, "Root Function Table Entry Position:    {}", self.root_function_table_entry_position)?;
        writeln!(f, "Root Constant Table Entry Position:    {}", self.root_constant_table_entry_position)?;
        writeln!(f, "Root Group Table Entry Position:       {}", self.root_group_table_entry_position)?;
        writeln!(f, "Region Table Entry Position:           {}", self.region_table_entry_position)?;

        Ok(())
    }
}
