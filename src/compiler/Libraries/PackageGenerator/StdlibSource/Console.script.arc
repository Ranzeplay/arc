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

	@DeclaratorOnly
	@SymbolId(0xa4)
	public func ReadInteger(): val int {}

	@DeclaratorOnly
	@SymbolId(0xa5)
	public func PrintDecimal(const d: ref decimal): ref none {}

	@DeclaratorOnly
	@SymbolId(0xa6)
	public func ReadDecimal(): val decimal {}
}
