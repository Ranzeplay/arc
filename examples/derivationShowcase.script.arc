link Arc::Std::Compilation;
link Arc::Std::Console;

namespace Program {
    public group Master {
        public field var a: val int;
    }

    public group Slave : Master {
        public func f(var self: val Slave): int {
            return self.a;
        }

        public constructor(var self: val Slave): Slave {
            self.a = 42;

            return self;
        }
    }

    @Entrypoint
    public func main(var args: val string[]): int {
        var s: val Slave = new Slave();

        call PrintInteger(s.a);

        return 0;
    }
}
