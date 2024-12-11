using Newtonsoft.Json.Linq;
using System.Text;

namespace GetGitHubCodeLines.Utilities
{
    public class JArrayParser
    {
        public static string? ParseLangAndLines(JArray json, Dictionary<string, int> dict)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < json.Count; i++)
            {
                try
                {
                    string key = json[i]["language"]!.ToString();
                    int value = int.Parse(json[i]["lines"]!.ToString());
                    sb.AppendLine($" > {key}: {value}");

                    if (!dict.TryAdd(key, value)) dict[key] += value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка парсинга JArray: " + ex.Message);
                    return null;
                }
            }

            return sb.ToString();
        }

        public static string[] ParseNames(JArray json)
        {
            try
            {
                var result = new string[json.Count].Select((x, i) => json[i]["name"]!.ToString()).ToArray();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка парсинга JArray: " + ex.Message);
                return [];
            }
        }
    }
}
