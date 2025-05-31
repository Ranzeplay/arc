link Arc::Std;

namespace Program {
	@Export
	@Entrypoint
	public func main(var args: val string[]): val int {
		call [Console].PrintLn("Hello, world!");
		return 0;
	}
}
