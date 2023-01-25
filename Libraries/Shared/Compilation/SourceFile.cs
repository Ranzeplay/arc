using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Compilation
{
    public class SourceFile
    {
        public string FilePath { get; }

        public string Content { get; }

        public SourceFile(string path, string content)
        {
            FilePath = path;
            Content = content;
        }
    }
}
