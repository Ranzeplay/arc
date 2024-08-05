link Arc::Std;

namespace Arc::Program {
	@Export
	public group ArcExample {
		@Getter
		public field const foo: val string;
		@Accessor
		public field var bar: val string;

		private func eval(): val bool {
			return false;
		}

		public func empty(): val none {}
	}

	@Export
	@Entrypoint
	public func main(var args: val string[]): val none {
		var a: val int;
		a = [Console].readLn();
		var b: val infer;
		a = 4;
		b = 6;
		const c: val infer;
		if (a > b) { c = a + b; }
		elif (a < b) { c = a * b; }
		else { c = a - b; }

		call [Console].PrintLn("Hello, world!");

		return 0;
	}
}
