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

namespace AvaxTelegramBot.Model
{
    public static class Handle
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    //await botClient.SendTextMessageAsync(message.Chat, "Бот Avax\nВыдает информацию о новых появившихся контрактах (адрес контракта, ссылку на контракт)");
                    // await botClient.SendTextMessageAsync(message.Chat, "/start - повторить сообщение\n/contracts - информация о контрактах\n");
                    await HandleEveryTenSecondsMessage(botClient, update);
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        private static async Task HandleEveryTenSecondsMessage(ITelegramBotClient botClient, Update update)
        {
            System.Timers.Timer Timer1 = new System.Timers.Timer();
            Timer1.Interval = 1000;
            Timer1.Elapsed += async (sender, e) => await HandleTimer(botClient, update);
            Timer1.Start();
        }
        private static async Task HandleTimer(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine("\nHenlo");
            await botClient.SendTextMessageAsync(update.Message.Chat, "Привет-привет!!");
            //throw new NotImplementedException();
        }
    }
}
