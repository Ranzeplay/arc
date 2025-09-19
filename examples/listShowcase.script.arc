link Arc::Std::Collection;
link Arc::Std::Compilation;
link Arc::Std::Console;

namespace Arc::Program {
    @Entrypoint
    public func main(const args: string[]): int {
        const list: List<string>;
        list = new List<string>(args);
        
        var size: int;
        size = list.getSize();

        call PrintInteger(size);
        call PrintString("\n");
        
        return 0;
    }
}
