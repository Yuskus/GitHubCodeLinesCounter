namespace GetGitHubCodeLines.Utilities
{
    public class InOutTool
    {
        public static async Task Output(string text, int delay = 1000)
        {
            await Task.Delay(delay);
            Console.WriteLine(text);
        }

        public static string InputText(string aboutInput)
        {
            Console.WriteLine(aboutInput);

            while (true)
            {
                string? input = Console.ReadLine();
                if (input is not null) return input;
                Console.WriteLine("Попробуйте ещё раз."); 
            }
        }
    }
}
