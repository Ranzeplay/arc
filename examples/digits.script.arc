link Arc::Std::Console;
link Arc::Std::Compilation;

namespace Program
{
   	@Entrypoint
   	public func main(var args: val string[]): val int
   	{
		var number: val int;
	  	number = ReadInteger();

		call PrintString("Number: ");
		call PrintInteger(number);
		call PrintString("\n");

	  	var digits: val int;
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