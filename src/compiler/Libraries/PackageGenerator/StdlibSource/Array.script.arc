link Arc::Std::Compilation;

namespace Arc::Std::Array
{
	@DeclaratorOnly
	@SymbolId(0xc1)
	public func CreateIntArray(): val int[] {}

	@DeclaratorOnly
	@SymbolId(0xc2)
	public func PushIntArray(var array: ref int[], const i: val int): ref none {}

	@DeclaratorOnly
	@SymbolId(0xc3)
	public func RemoveElementFromIntArray(var array: ref int[], const index: ref int): ref none {}

	@DeclaratorOnly
	@SymbolId(0xc4)
	public func GetIntArraySize(const arr: ref int[]): val int {}
}