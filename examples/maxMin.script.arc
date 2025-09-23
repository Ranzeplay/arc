link Arc::Std::Console;
link Arc::Std::Compilation;
link Arc::Std::Array;

namespace Program
{
	@Entrypoint
	public func main(var args: string[]): int
	{
		const count: int;
		count = ReadInteger();

		var array: int[];
		array = CreateIntArray();

		var i: int;

		i = 0;
		while(i < count)
		{
			call PushIntArray(array, ReadInteger());
			i = i + 1;
		}

		var max: int;
		var min: int;
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

	private func OutputNumber(const hint: string, const n: int): none
	{
		call PrintString(hint);
		call PrintInteger(n);
		call PrintString("\n");
		return;
	}
}