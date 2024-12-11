using GetGitHubCodeLines.Abstractions;
using GetGitHubCodeLines.Implementations;
using GetGitHubCodeLines.Objects;
using GetGitHubCodeLines.Utilities;

namespace GetGitHubCodeLines
{
    internal class Program
    {
        static async Task Main()
        {
            // init username
            string username = InOutTool.InputText("Привет, это приложение для подсчёта количества строк кода в репозитории. \n" +
                                                  "Для начала работы укажите имя пользователя на GitHub.");

            // init multi-output
            IPrintCollection printList = new PrinterCollection()
                                         .AddConsole()
                                         .AddFile($"{username.ToLower()}_lines_of_code.txt");

            // info
            await InOutTool.Output("Отправлен запрос на GitHub, ожидайте...", 0);

            // http request to github
            var github = new GitHubCatcher(username);
            string[] repos = await github.RequestToApi();

            // get dictionary
            var reposInfo = new UserRepoCollectionInfo(printList);
            var reposCollection = reposInfo.GetTotalRepositoriesInfo();

            // info
            await InOutTool.Output("Ответ от GitHub получен.", 0);
            await InOutTool.Output("Отправлен запрос на Codetabs Api.");
            await InOutTool.Output("Внимание, у Codetabs Api ограничение - не более одного запроса в 5 секунд.");
            await InOutTool.Output("Поэтому анализ профиля на GitHub займёт некоторое время.");
            await InOutTool.Output("Ожидание ответа с Codetabs Api...\n");

            printList.WriteLineAll($"Анализ репозитория пользователя {username}\n");

            // http request to codetabs
            var codetabs = new CodetabsCatcher(username, repos, printList);
            await codetabs.RequestToApi(reposCollection);

            // info
            await InOutTool.Output("\nВсе репозитории проанализированы!\n", 0);
            await InOutTool.Output("Итог:\n");

            reposInfo.PrintFullInfo();

            await InOutTool.Output("\nГотово!", 0);

            printList.Dispose();

            await InOutTool.Output("Для завершения работы приложения нажмите любую клавишу.");

            Console.ReadKey(true);

            await InOutTool.Output("Приложение закроется через 5 секунд. До свидания!", 0);

            await Task.Delay(5000);
        }
    }
}
