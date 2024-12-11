using GetGitHubCodeLines.Abstractions;

namespace GetGitHubCodeLines.Objects
{
    public class UserRepoCollectionInfo(IPrintCollection printers)
    {
        private readonly Dictionary<string, int> _totalRepositoriesInfo = [];
        private readonly IPrintCollection _printers = printers;

        public Dictionary<string, int> GetTotalRepositoriesInfo()
        {
            return _totalRepositoriesInfo;
        }

        public void PrintFullInfo()
        {
            var pairs = _totalRepositoriesInfo.OrderByDescending(x => x.Value).ToArray();

            foreach (var val in pairs)
            {
                _printers.WriteLineAll($" > {val.Key,-30}: {val.Value,5}");
            }
        }
    }
}
