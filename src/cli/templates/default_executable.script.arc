link Arc::Std::Compilation;
link Arc::Std::Console;

namespace Program {
	@Export
	@Entrypoint
	public func main(var args: string[]): int {
		call PrintString("Hello, world!");
		return 0;
	}
}
