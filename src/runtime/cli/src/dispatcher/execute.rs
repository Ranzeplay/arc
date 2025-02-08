use executor::launch;
use shared::models::package::Package;

pub fn execute(package: Package, verbose: bool) -> i32 {
    let result = launch(package, verbose);
    result.unwrap_or_else(|e| {
        log::error!("Error: {}", e);
        0xffff
    })
}
