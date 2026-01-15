link Arc::Std::Console;
link Arc::Std::Compilation;

namespace Program
{
	@Entrypoint
	public func main(const args: string[]): int
	{
		call PrintString("Hello, world!\n");

		return 0;
	}	
}
