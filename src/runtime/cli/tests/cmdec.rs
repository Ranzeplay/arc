use std::path::PathBuf;
use std::process::{Command, Output};
use std::sync::atomic::{AtomicUsize, Ordering};

struct Fixture(PathBuf);

impl Fixture {
    fn new(bytes: &[u8]) -> Self {
        static NEXT: AtomicUsize = AtomicUsize::new(0);
        let path = std::env::temp_dir().join(format!(
            "arc-cmdec-{}-{}",
            std::process::id(),
            NEXT.fetch_add(1, Ordering::Relaxed)
        ));
        std::fs::write(&path, bytes).unwrap();
        Self(path)
    }

    fn run(&self, command: &str, args: &[&str]) -> Output {
        Command::new(env!("CARGO_BIN_EXE_arc_cli"))
            .env("RUST_LOG", "info")
            .env("NO_COLOR", "1")
            .arg(command)
            .arg(&self.0)
            .args(args)
            .output()
            .unwrap()
    }
}

impl Drop for Fixture {
    fn drop(&mut self) {
        let _ = std::fs::remove_file(&self.0);
    }
}

fn package() -> Vec<u8> {
    let mut bytes = b"GINKGO\0\0".to_vec();
    bytes.extend(2u32.to_le_bytes());
    bytes.push(0); // Library.
    bytes.extend(0u64.to_le_bytes()); // Empty package name.
    bytes.extend(1i64.to_le_bytes());
    bytes.extend([0; 44]); // Entrypoint, alignment and reserved positions.
    bytes.extend(1u32.to_le_bytes()); // One namespace symbol.
    bytes.extend(42u32.to_le_bytes());
    bytes.push(1);
    bytes.extend(1u32.to_le_bytes()); // One empty constant pool.
    bytes.extend(0u32.to_le_bytes());
    let payload = &bytes[12..];
    let mut envelope = b"GINKGO\0\0\x02\0\0\0\0".to_vec();
    envelope.extend((payload.len() as u64).to_le_bytes());
    envelope.extend((payload.len() as u64).to_le_bytes());
    envelope.extend(payload);
    envelope
}

fn stderr(output: &Output) -> String {
    String::from_utf8_lossy(&output.stderr).into_owned()
}

#[test]
fn auto_detects_ginkgo_and_respects_display_options() {
    let fixture = Fixture::new(&package());
    let quiet = fixture.run("decode", &[]);
    assert!(quiet.status.success(), "{}", stderr(&quiet));
    assert!(!stderr(&quiet).contains("=== Symbols"));
    assert!(!stderr(&quiet).contains("Ginkgo format"));

    let output = fixture.run("decode", &["-d", "-s", "-c", "-i"]);
    assert!(output.status.success(), "{}", stderr(&output));
    let text = stderr(&output);
    for expected in [
        "Ginkgo package",
        "#42: Namespace",
        "=== Constant pool 0",
        "=== Instructions",
    ] {
        assert!(text.contains(expected), "{}", text);
    }
}

#[test]
fn reads_optional_source_information_and_reports_invalid_metadata() {
    let fixture = Fixture::new(&package());
    let source = Fixture::new(
        br#"{"instructionSet":"arc-ginkgo","symbolMapping":{"42":"Test"},"functionSources":{}}"#,
    );
    let output = fixture.run("decode", &["-s", "-m", source.0.to_str().unwrap()]);
    assert!(output.status.success(), "{}", stderr(&output));
    assert!(stderr(&output).contains("#42 (Test): Namespace"));

    for bytes in [
        b"invalid".as_slice(),
        br#"{"instructionSet":"legacy","symbolMapping":{},"functionSources":{}}"#,
    ] {
        let source = Fixture::new(bytes);
        let output = fixture.run("decode", &["-m", source.0.to_str().unwrap()]);
        assert!(!output.status.success());
        assert!(stderr(&output).contains("Failed to read Ginkgo source information"));
    }
}

#[test]
fn reports_invalid_packages_and_unsupported_execution_without_panicking() {
    for bytes in [
        vec![],
        b"GINKGO\0\0".to_vec(),
        b"GINKGO\0\0\x03\0\0\0".to_vec(),
    ] {
        let output = Fixture::new(&bytes).run("decode", &[]);
        assert!(!output.status.success());
        assert!(!stderr(&output).contains("panicked"));
    }
    let output = Fixture::new(&package()).run("execute", &[]);
    assert!(!output.status.success());
    assert!(stderr(&output).contains("Ginkgo execution is not supported"));
}
