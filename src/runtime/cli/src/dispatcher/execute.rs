use executor::launch;
use shared::models::package::Package;

pub fn execute(package: Package, verbose: bool, repeat: u32, args: Vec<String>) -> i32 {
    for _ in 0..repeat {
        let result = launch(package.clone(), verbose, args.clone());
        if let Err(e) = result {
            log::error!("Error: {}", e);
            return 0xffff;
        }
    }

    0
}
