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



using Newtonsoft.Json.Linq;

using Telegram.Bot.Types.ReplyMarkups;
using AvaxTelegramBot.Model.Commands;
using AvaxTelegramBot;

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

                foreach (var command in Bot.Commands)
                {
                    if (command.Contains(message))
                        await command.Execute(botClient, update);
                }
                //await botClient.SendTextMessageAsync(message.Chat, "/start - начать/прекратить информирование");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        //TO DO парсинг контрактов по блоку https://snowtrace.io/txsInternal
        //TO DO возможно перенести функции в класс Bot


    }
}