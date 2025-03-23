use std::rc::Rc;
use executor::launch;
use shared::models::options::launch_options::LaunchOptions;

pub fn execute(opt: LaunchOptions) -> i32 {
    let opt = Rc::new(opt);

    for _ in 0..opt.repeat {
        let result = launch(Rc::clone(&opt));
        if let Err(e) = result {
            log::error!("Error: {}", e);
            return 0xffff;
        }
    }

    0
}
