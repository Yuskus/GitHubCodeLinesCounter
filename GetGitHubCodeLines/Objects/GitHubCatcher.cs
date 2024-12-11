using GetGitHubCodeLines.Utilities;
using Newtonsoft.Json.Linq;

namespace GetGitHubCodeLines.Objects
{
    public class GitHubCatcher(string username) : HttpCatcher
    {
        private readonly string _url = $"https://api.github.com/users/{username}/repos";

        public async Task<string[]> RequestToApi()
        {
            string? responseBody = await HttpRequest(_url);
            if (responseBody is null) return [];
            return JArrayParser.ParseNames(JArray.Parse(responseBody));
        }
    }
}
