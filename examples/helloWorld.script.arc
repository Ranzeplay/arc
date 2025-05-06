link Arc::Std::Console;
link Arc::Std::Compilation;

namespace Program
{
   	@Entrypoint
   	public func main(var args: val string[]): val int
   	{
      	call PrintString("Hello, world!\n");

      	return 0;
   	}	
}