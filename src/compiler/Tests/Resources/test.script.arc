link Arc::Std;

namespace Arc::Program {
	@Export
	public group ArcExample<T> {
		@Getter
		public field const foo: T;
		@Accessor
		public field var bar: string;

		private func eval(): bool {
			return false;
		}

		public func empty(): none {}

		public func ctor(): none {
			foo = "foo";
			bar = "bar";
		}
	}

	@Export
	@Entrypoint
	public func main(var args: string[]): none {
		var a: int = 4;
		a = [Console].readLn();
		var b: infer;
		b = 6;
		const c: infer;
		if (a > b) { c = a + b; }
		elif (a < b) { c = a * b; }
		else { c = a - b; }

		call [Console].PrintLn("Hello, world!");

		var d: ArcExample<string>;
		d = new ArcExample<string>();
		
		const fn: func = lambda (const a: int, const b: int): int => {
            if (a > b) {
                return a;
            } else {
                return b;
            }
        };
        
        const result: int = fn(50, 0);

		return result;
	}
}
