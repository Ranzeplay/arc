link Arc::Std;

namespace Arc::Program {
	@Export
	public group ArcExample<T> {
		@Getter
		public field const foo: val T;
		@Accessor
		public field var bar: val string;

		private func eval(): val bool {
			return false;
		}

		public func empty(): val none {}

		public func ctor(): val none {
			foo = "foo";
			bar = "bar";
		}
	}

	@Export
	@Entrypoint
	public func main(var args: val string[]): val none {
		var a: val int = 4;
		a = [Console].readLn();
		var b: val infer;
		b = 6;
		const c: val infer;
		if (a > b) { c = a + b; }
		elif (a < b) { c = a * b; }
		else { c = a - b; }

		call [Console].PrintLn("Hello, world!");

		var d: val ArcExample<val string>;
		d = new ArcExample<val string>();

		return 0;
	}
}
