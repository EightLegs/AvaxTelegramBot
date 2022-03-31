using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

using AvaxTelegramBot.Model;

using Microsoft.Extensions.Configuration;

namespace AvaxTelegramBot
{

    class Program
    {

        static void Main(string[] args)
        {
            // получаем конфигурацию из файла appsettings.json
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");
            string keyToken = config.GetSection("botData")["Key"];

            //ITelegramBotClient bot = botClientMake();
            
            ITelegramBotClient bot = new Bot(keyToken);

            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                Handle.HandleUpdateAsync,
                Handle.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}

/*        static ITelegramBotClient botClientMake()
        {
            // получаем конфигурацию из файла appsettings.json
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            // получаем строку подключения из файла appsettings.json
            string connectionString = config.GetConnectionString("DefaultConnection");
            string keyToken = config.GetSection("botData")["Key"];
            ITelegramBotClient bot = new TelegramBotClient(keyToken);
            return bot;
        }*/