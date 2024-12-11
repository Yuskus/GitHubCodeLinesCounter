namespace GetGitHubCodeLines.Abstractions
{
    public interface IPrintCollection : IDisposable
    {
        IPrintCollection AddConsole();
        IPrintCollection AddFile(string filePath);
        void ClearPrinters(bool stayConsole);
        Dictionary<string, IPrint> GetPrintersCopy();
        void WriteAll(string? text);
        void WriteLineAll(string? text);
    }
}
