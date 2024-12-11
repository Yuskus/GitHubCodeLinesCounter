using GetGitHubCodeLines.Abstractions;
using GetGitHubCodeLines.Utilities;
using Newtonsoft.Json.Linq;

namespace GetGitHubCodeLines.Objects
{
    public class CodetabsCatcher(string username, string[] repos, IPrintCollection printerList) : HttpCatcher
    {
        private readonly IPrintCollection _printerList = printerList;
        private readonly string? _username = username;
        private readonly string[] _repos = repos;

        private string BuildUrl(string repoName)
        {
            return $"https://api.codetabs.com/v1/loc?github={_username}/{repoName}";
        }

        public async Task RequestToApi(Dictionary<string, int> dict)
        {
            for (int i = 0; i < _repos.Length; i++)
            {
                await OneRequest(_repos[i], dict);
            }
        }

        public async Task OneRequest(string repoName, Dictionary<string, int> dict)
        {
            string? responseBody = null;

            while (responseBody is null)
            {
                responseBody = await HttpRequest(BuildUrl(repoName));
            }

            string? langAndLines = JArrayParser.ParseLangAndLines(JArray.Parse(responseBody), dict);

            if (langAndLines is null) return;

            _printerList.WriteLineAll($"{repoName}:\n{langAndLines}");
        }
    }
}
