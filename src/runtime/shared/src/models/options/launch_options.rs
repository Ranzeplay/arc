use crate::models::package::Package;
use std::rc::Rc;

pub struct LaunchOptions {
    pub package: Rc<Package>,
    pub verbose: bool,
    pub repeat: u32,
    pub args: Vec<String>,
}
