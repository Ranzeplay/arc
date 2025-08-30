link Arc::Std::Compilation;

namespace Arc::Std::Array
{
	@DeclaratorOnly
	@SymbolId(0xc1)
	public func CreateArray<T>(): val T[] {}

	@DeclaratorOnly
	@SymbolId(0xc2)
	public func PushArray<T>(var array: ref T[], const i: val int): ref none {}

	@DeclaratorOnly
	@SymbolId(0xc3)
	public func RemoveElementFromArray<T>(var array: ref T[], const index: ref int): ref none {}

	@DeclaratorOnly
	@SymbolId(0xc4)
	public func GetArraySize<T>(const arr: ref T[]): val int {}

	@DeclaratorOnly
	@SymbolId(0xc5)
	public func ResizeArray<T>(const arr: ref T[], const capacity: val int): ref none {}

	@DeclaratorOnly
	@SymbolId(0xc6)
	public func CreateArray<T>(const size: val int): val T[] {}

	# Preconditions:
	# - index is within bounds (0 <= index <= size of array)
	# - array has enough capacity to hold one more element
	public func InsertElementIntoArray<T>(var array: ref T[], const index: val int, const element: val T): ref none
	{
		var size: val int;
		size = GetArraySize<T>(array);
		if (index < 0 || index > size)
		{
			# TODO: Error handling for out-of-bounds index
		}
		for(var i: val int = size; i > index; i = i - 1)
        {
            array[i] = array[i - 1];
        }
		array[index] = element;
		return none;
	}
}
