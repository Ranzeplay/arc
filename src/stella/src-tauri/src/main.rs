#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

mod discovery;
mod runner;

fn main() {
    tauri::Builder::default()
        .plugin(tauri_plugin_dialog::init())
        .manage(runner::Runs::default())
        .invoke_handler(tauri::generate_handler![
            discovery::bootstrap,
            discovery::discover,
            discovery::source_files,
            discovery::read_source,
            runner::start_run,
            runner::stop_run,
            runner::export_run,
            runner::delete_run,
            runner::clear_runs,
        ])
        .build(tauri::generate_context!())
        .expect("Unable to start Stella")
        .run(|app, event| {
            if matches!(event, tauri::RunEvent::ExitRequested { .. }) {
                use tauri::Manager;
                app.state::<runner::Runs>().stop_all();
            }
        });
}
