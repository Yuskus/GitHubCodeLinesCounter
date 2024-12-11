using GetGitHubCodeLines.Abstractions;

namespace GetGitHubCodeLines.Implementations
{
    public class Printer : IPrint, IDisposable
    {
        private readonly TextWriter _writer;
        public Printer()
        {
            _writer = Console.Out;
        }
        public Printer(TextWriter writer)
        {
            _writer = writer;
        }
        public void WriteLine(string? text)
        {
            _writer.WriteLine(text);
        }

        public void Write(string? text)
        {
            _writer.Write(text);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer?.Dispose();
            }
        }
    }
}
