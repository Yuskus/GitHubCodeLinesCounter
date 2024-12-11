namespace GetGitHubCodeLines.Abstractions
{
    public interface IPrint
    {
        void Write(string? text);
        void WriteLine(string? text);
    }
}