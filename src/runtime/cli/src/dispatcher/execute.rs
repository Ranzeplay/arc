use executor::launch;
use shared::models::package::Package;

pub fn execute(package: Package, verbose: bool, repeat: u32) -> i32 {
    for _ in 0..repeat {
        let result = launch(package.clone(), verbose);
        if let Err(e) = result {
            log::error!("Error: {}", e);
            return 0xffff;
        }
    }

    0
}
