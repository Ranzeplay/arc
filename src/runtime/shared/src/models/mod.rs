pub mod descriptors;
pub mod display;
pub mod encodings;
pub mod execution;
pub mod instruction;
pub mod options;
pub mod package;
pub mod source_info;

// Register instructions before generating the enum. Separate groups prevent rustfmt reordering.
pub mod instructions;

pub mod instruction_type;
