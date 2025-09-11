link Arc::Std::Compilation;

namespace Arc::Std::Console
{
	@DeclaratorOnly
	@SymbolId(0xa1)
	public func PrintString(const s: string): none {}

	@DeclaratorOnly
	@SymbolId(0xa2)
	public func PrintInteger(const i: int): none {}

	@DeclaratorOnly
	@SymbolId(0xa3)
	public func ReadString(): string {}

	@DeclaratorOnly
	@SymbolId(0xa4)
	public func ReadInteger(): int {}

	@DeclaratorOnly
	@SymbolId(0xa5)
	public func PrintDecimal(const d: decimal): none {}

	@DeclaratorOnly
	@SymbolId(0xa6)
	public func ReadDecimal(): decimal {}
}
