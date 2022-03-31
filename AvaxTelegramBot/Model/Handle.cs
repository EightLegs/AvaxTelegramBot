using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

using System.Timers;
using System.Net;

namespace AvaxTelegramBot.Model
{
    public static class Handle
    {
        //public static bool ShouldWork { get; set; } = false;
        public static System.Timers.Timer Timer1 { get; set; }
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await HandleEveryTenSecondsMessage(botClient, update);
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "/start - начать/прекратить информирование");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        private static async Task HandleEveryTenSecondsMessage(ITelegramBotClient botClient, Update update)
        {
            if (Timer1 != null && Timer1.Enabled)
            {
                Timer1.Stop();
                Timer1.Dispose();
            }

            else
            {
                Timer1 = new System.Timers.Timer(1000);
                Timer1.Elapsed += async (sender, e) => await HandleTimer(botClient, update);
                Timer1.Start();
            }
        }
        private static async Task HandleTimer(ITelegramBotClient botClient, Update update)
        {
            using (WebClient wc = new WebClient())
            {
                //var json = wc.DownloadString("");
                // Or you can get the file content without saving it
                string htmlCode = wc.DownloadString("https://snowtrace.io/blocks");
                int b = 5;
            }
            await botClient.SendTextMessageAsync(update.Message.Chat, "Привет-привет!!");
        }
    }
}
