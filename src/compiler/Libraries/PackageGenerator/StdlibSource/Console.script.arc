link Arc::Std::Compilation;

namespace Arc::Std::Console
{
	@DeclaratorOnly
	@SymbolId(0xa1)
	public func PrintString(const s: ref string): ref none {}

	@DeclaratorOnly
	@SymbolId(0xa2)
	public func PrintInteger(const i: ref int): ref none {}

	@DeclaratorOnly
	@SymbolId(0xa3)
	public func ReadString(): val string {}
}
