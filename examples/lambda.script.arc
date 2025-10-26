link Arc::Std::Console;
link Arc::Std::Compilation;

namespace Program
{
	@Entrypoint
	public func main(var args: string[]): int
	{
		const f: func = lambda (): none => {
            call PrintString("Hello, world!\n");
            return;
        };

        call f();

		return 0;
	}	
}
