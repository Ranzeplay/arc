link Arc::Std::Compilation;

namespace Arc::Std::Array
{
	@DeclaratorOnly
	@SymbolId(0xc1)
	public func CreateArray<T>(): T[] {}

	@DeclaratorOnly
	@SymbolId(0xc2)
	public func PushArray<T>(var array: T[], const i: int): none {}

	@DeclaratorOnly
	@SymbolId(0xc3)
	public func RemoveElementFromArray<T>(var array: T[], const index: int): none {}

	@DeclaratorOnly
	@SymbolId(0xc4)
	public func GetArraySize<T>(const arr: T[]): int {}

	@DeclaratorOnly
	@SymbolId(0xc5)
	public func ResizeArray<T>(const arr: T[], const capacity: int): none {}

	@DeclaratorOnly
	@SymbolId(0xc6)
	public func CreateArray<T>(const size: int): T[] {}

	# Preconditions:
	# - index is within bounds (0 <= index <= size of array)
	# - array has enough capacity to hold one more element
	public func InsertElementIntoArray<T>(var array: T[], const index: int, const element: T): none
	{
		var size: int;
		size = GetArraySize<T>(array);
		if (index < 0 || index > size)
		{
			# TODO: Error handling for out-of-bounds index
		}
		for(var i: int = size; i > index; i = i - 1)
        {
            array[i] = array[i - 1];
        }
		array[index] = element;
		return none;
	}
}
