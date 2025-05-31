link Arc::Std::Console;
link Arc::Std::Compilation;
link Arc::Std::Array;

namespace Program
{
   	@Entrypoint
   	public func main(var args: val string[]): val int
   	{
		const count: val int;
		count = ReadInteger();

		var array: val int[];
		array = CreateIntArray();

		var i: val int;

		i = 0;
		while(i < count)
		{
			call PushIntArray(array, ReadInteger());
			i = i + 1;
		}

		var max: val int;
		var min: val int;
		max = array[0];
		min = array[0];

		i = 0;
		while(i < count)
		{
			if(array[i] > max)
			{
				max = array[i];
			}

			if(array[i] < min)
			{
				min = array[i];
			}

			i = i + 1;
		}

		call OutputNumber("Max: ", max);
		call OutputNumber("Min: ", min);

	  	return 0;
   	}

	private func OutputNumber(const hint: ref string, const n: ref int): ref none
	{
		call PrintString(hint);
		call PrintInteger(n);
		call PrintString("\n");
		return;
	}
}