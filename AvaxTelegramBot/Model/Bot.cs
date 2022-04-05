using Telegram.Bot;
using AvaxTelegramBot.Model.Commands;

namespace AvaxTelegramBot.Model
{
    public class Bot : TelegramBotClient
    {
        public ulong? LastBlockID { get; set; } = null;
        public string? ApiKey { get; set; }
        public string KeyToken { get; set; }
        public static bool BlockIDWait { get; set; } = false;
        public static System.Timers.Timer Timer1 { get; set; }

        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public Bot(string token, string apikey, HttpClient? httpClient = null, string? baseUrl = null) : base(token, httpClient, baseUrl)
        {
            KeyToken = token;
            ApiKey = apikey;

            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            commandsList.Add(new StopCommand());
            commandsList.Add(new BlockInfoCommand());
        }
    }
}
