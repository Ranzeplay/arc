link Arc::Std::Console;
link Arc::Std::Compilation;

namespace Arc::Program {
	# @Export
	@Entrypoint
	public func main(var args: string[]): int {
		var i: int;
		i = 0;
		while (i < 32)
		{
			const value: int;
			value = fib(i);
			call PrintInteger(value);
			call PrintString("\n");
			i = i + 1;
		}

		return 0;
	}

	public func fib(const n: int): int
	{
		if (n <= 1)
		{
			return 1;
		}
		else
		{
			return fib(n - 1) + fib(n - 2);
		}
	}
}
