using GetGitHubCodeLines.Abstractions;

namespace GetGitHubCodeLines.Implementations
{
    public class PrinterCollection : IPrintCollection
    {
        private readonly Dictionary<string, IPrint> _printers = [];

        public IPrintCollection AddConsole()
        {
            if (!_printers.ContainsKey("Console"))
            {
                _printers.Add("Console", new Printer());
            }

            return this;
        }

        public IPrintCollection AddFile(string filePath)
        {
            if (!_printers.ContainsKey(filePath))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                _printers.Add(filePath, new Printer(File.CreateText(path)));
            }

            return this;
        }

        public void ClearPrinters(bool stayConsole = false)
        {
            _printers.Clear();
            if (stayConsole) AddConsole();
        }

        public void Dispose()
        {
            foreach (var value in _printers.Values)
            {
                var item = value as IDisposable;
                item?.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public Dictionary<string, IPrint> GetPrintersCopy()
        {
            return new Dictionary<string, IPrint>(_printers);
        }

        public void WriteAll(string? text)
        {
            foreach (IPrint printer in _printers.Values)
            {
                printer.Write(text);
            }
        }

        public void WriteLineAll(string? text)
        {
            foreach (IPrint printer in _printers.Values)
            {
                printer.WriteLine(text);
            }
        }
    }
}
