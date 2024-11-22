using Newtonsoft.Json.Linq;

namespace GetGitHubCodeLines
{
    internal class Program
    {
        static readonly Dictionary<string, int> dict = [];
        static async Task Main()
        {
            Console.WriteLine("Привет, это приложение для подсчёта количества строк кода в репозитории.");

            string username = EnterText("Для начала работы укажите имя пользователя на GitHub.");

            await OutputWithPause("Отправлен запрос на GitHub, ожидайте...");

            string[] repos = await ToGitHub(username);

            await OutputWithPause("Ответ от GitHub получен.");
            await OutputWithPause("Отправлен запрос на Codetabs Api.");
            await OutputWithPause("Внимание, у Codetabs Api есть ограничение на количество запросов - не более одного в 5 секунд.");
            await OutputWithPause("Поэтому анализ профиля на GitHub займёт некоторое время.");
            await OutputWithPause("Ожидание ответа с Codetabs Api...");

            await ToCodetabsApi(username, repos);

            Console.WriteLine();
            Console.WriteLine("Все репозитории проанализированы!");
            Console.WriteLine();
            await OutputWithPause("Итог:");
            Console.WriteLine();

            PrintFullInfo();

            Console.WriteLine();
            Console.WriteLine("Готово!");
            await OutputWithPause("Для завершения работы приложения нажмите любую клавишу.");

            Console.ReadKey(true);

            Console.WriteLine("Приложение закроется через 5 секунд. До свидания!");

            await Task.Delay(5000);
        }

        private static async Task OutputWithPause(string text, int delay = 1000)
        {
            await Task.Delay(delay);
            Console.WriteLine(text);
        }

        private static string EnterText(string aboutInput)
        {
            Console.WriteLine(aboutInput);
            while (true)
            {
                string? input = Console.ReadLine();
                if (input is not null) return input;
                Console.WriteLine("Попробуйте ещё раз.");
            }
        }

        static async Task<string[]> ToGitHub(string username)
        {
            string url = $"https://api.github.com/users/{username}/repos";
            string? responseBody = await HttpRequest(url) ?? throw new Exception("Ошибка! Не можем связаться с сайтом.");

            try
            {
                JArray array = JArray.Parse(responseBody);
                return new string[array.Count].Select((x, i) => array[i]["name"]!.ToString()).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                return [];
            }
        }

        private static async Task ToCodetabsApi(string username, string[] repos)
        {
            for (int i = 0; i < repos.Length; i++)
            {
                await GetRepoStatistics(username, repos[i]);
            }
        }

        private static async Task GetRepoStatistics(string username, string reponame)
        {
            string url = $"https://api.codetabs.com/v1/loc?github={username}/{reponame}";
            string? responseBody = await HttpRequest(url) ?? throw new Exception("Ошибка! Не можем связаться с сайтом!");

            try
            {
                JArray array = JArray.Parse(responseBody);

                Console.WriteLine("\n" + reponame + ":");

                for (int i = 0; i < array.Count; i++)
                {
                    string key = array[i]["language"]!.ToString();
                    int value = int.Parse(array[i]["lines"]!.ToString());

                    if (!dict.TryAdd(key, value)) dict[key] += value;

                    Console.WriteLine($" > {key}: {value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static void PrintFullInfo()
        {
            var pairs = dict.OrderByDescending(x => x.Value).ToArray();

            foreach (var val in pairs)
            {
                Console.WriteLine($" > {val.Key,-30}: {val.Value,5}");
            }
        }

        private static async Task<string?> HttpRequest(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке запроса: {ex.Message}");
                return null;
            }
        }
    }
}
