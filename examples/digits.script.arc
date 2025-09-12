link Arc::Std::Console;
link Arc::Std::Compilation;

namespace Program
{
	 	@Entrypoint
	 	public func main(var args: string[]): int
		{
			var number: int;
			number = ReadInteger();

			call PrintString("Number: ");
			call PrintInteger(number);
		call PrintString("\n");

	  	var digits: int;
	  	digits = 0;
	  	while (number <> 0)
	  	{
			number = number / 10;
			digits = digits + 1;
	  	}

		call PrintString("Digits: ");
		call PrintInteger(digits);
		call PrintString("\n");

	  	return 0;
   	}
}