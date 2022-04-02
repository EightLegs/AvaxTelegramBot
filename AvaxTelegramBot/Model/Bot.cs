using Telegram.Bot;
using AvaxTelegramBot.Model.Commands;

namespace AvaxTelegramBot.Model
{
    public class Bot : TelegramBotClient
    {
        //private static TelegramBotClient botClient;
        //private static List<Command> commandsList;
        public ulong? LastBlockID { get; set; } = null;
        public string? ApiKey { get; set; }
        public string KeyToken { get; set; }

        public Bot(string token, string apikey, HttpClient? httpClient = null, string? baseUrl = null) : base(token, httpClient, baseUrl)
        {
            KeyToken = token;
            ApiKey = apikey;
        }

       // public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();
        /*public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
                return botClient;
            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            //TODO: Add more commands

            botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/update");
         
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }*/

    }
}
