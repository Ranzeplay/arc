link Arc::Std::Console;
link Arc::Std::Compilation;

namespace Program
{
    public group G<T> {
        private field const predicate: func;

        public constructor(var self: G, const p: func): G
        {
            self.predicate = p;
            return self;
        }

        public func check(const self: G, const v: T): bool
        {
            return self.predicate(v);
        }
    }

	@Entrypoint
	public func main(var args: string[]): int
	{
		const f: func = lambda (const i: int): bool => {
            call PrintString("Checking\n");
            return i == 0;
        };

        const g: G<int> = new G(f);
        const result: bool = g.check(2);

		return 0;
	}
}
