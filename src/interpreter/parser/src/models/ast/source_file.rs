use crate::models::ast::function::Function;
use crate::models::ast::link_target::LinkTarget;
use crate::models::ast::procedure::Procedure;

pub struct SourceFile {
    pub links: Vec<LinkTarget>,
    pub functions: Vec<Function>,
    pub procedures: Option<Procedure>,
}
