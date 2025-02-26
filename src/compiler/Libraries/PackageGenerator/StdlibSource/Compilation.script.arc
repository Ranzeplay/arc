namespace Arc::Std::Compilation
{
	@SymbolId(0xb1)
	@WithAnnotationId(0xb11)
	public group Entrypoint {}

	@SymbolId(0xb2)
	@WithAnnotationId(0xb21)
	public group Export {}

	@SymbolId(0xb3)
	@WithAnnotationId(0xb31)
	public group Getter {}

	@SymbolId(0xb4)
	@WithAnnotationId(0xb41)
	public group Setter {}

	@SymbolId(0xb5)
	@WithAnnotationId(0xb51)
	public group Constructor {}

	@SymbolId(0xb6)
	@WithAnnotationId(0xb61)
	public group Destructor {}

	@SymbolId(0xb7)
	@WithAnnotationId(0xb71)
	public group symbolId
	{
		public const id: val int;
	}

	@SymbolId(0xb8)
	@WithAnnotationId(0xb81)
	public group DeclaratorOnly {}

	@SymbolId(0xb9)
	@WithAnnotationId(0xb91)
	public group WithAnnotation {}

	@SymbolId(0xba)
	@WithAnnotationId(0xba1)
	public group WithAnnotationId
	{
		public const id: val int;
	}

	@SymbolId(0xbb)
	@WithAnnotationId(0xbb1)
	public group NoType {}

	@SymbolId(0xbb)
	@WithAnnotationId(0xbb1)
	public group GlobalLinked {}
}
