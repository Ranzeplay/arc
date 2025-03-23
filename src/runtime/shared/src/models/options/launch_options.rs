use std::rc::Rc;
use crate::models::package::Package;

pub struct LaunchOptions {
    pub package: Rc<Package>,
    pub verbose: bool,
    pub repeat: u32,
    pub args: Vec<String>
}
