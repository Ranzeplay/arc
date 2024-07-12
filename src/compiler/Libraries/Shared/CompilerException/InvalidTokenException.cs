using Arc.Compiler.Shared.LexicalAnalysis;

namespace Arc.Compiler.Shared.CompilerException
{
    public class InvalidTokenException : Exception
    {
        private ExceptionDescription Description { get; } = new(ExceptionLevel.Error, ExceptionZone.LexicalAnalysis, 0);

        private TokenPosition? Position { get; }

        private string? Reason { get; }

        public override string Message => Description.ToString();

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

            Description = new(ExceptionLevel.Error, ExceptionZone.LexicalAnalysis, 0, GetDescription());
        }

        public string GetDescription()
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
}
