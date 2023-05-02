using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CompilerException
{
    public class InvalidTokenException : Exception
    {
        private TokenPosition? Position { get; }

        private string? Reason { get; }

        public override string Message
        {
            get
            {
                if (Position != null)
                {
                    var message = $"Invalid token in \"{Position.SourceFile.FilePath}\" at character index {Position.Position}";
                    if (Reason != null)
                    {
                        message += $"\nReason: {Reason}";
                    }

                    return message;
                }

                return "Invalid token, no further information";
            }
        }

        public InvalidTokenException() { }

        public InvalidTokenException(string message) : base(message) { }

        public InvalidTokenException(string message, Exception ex)
        : base(message, ex)
        {
        }

        public InvalidTokenException(TokenPosition position, string? reason = null)
        {
            Position = position;
            Reason = reason;
        }
    }
}
