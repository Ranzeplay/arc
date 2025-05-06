link Arc::Std::Console;

namespace Arc::Program {
	# @Export
	@Entrypoint
	public func main(var args: val string[]): val int {
		var i: val int;
		i = 0;
		while (i < 32)
		{
			const value: val int;
			value = fib(i);
			call PrintInteger(value);
			call PrintString("\n");
			i = i + 1;
		}

		return 0;
	}

	public func fib(const n: val int): val int
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
