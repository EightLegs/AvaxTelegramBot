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
                if(!Bot.BlockIDWait)
                {
                    try
                    {
                        ulong blockID = ulong.Parse(message.ToString());
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                        await botClient.SendTextMessageAsync(update.Message.Chat, ex.Message);
                    }
                }
                foreach (var command in Bot.Commands)
                {
                    if(!Bot.BlockIDWait)
                    {
                        var messageBlockID = new Message();
                        messageBlockID.Text = "/blockinfo";
                        
                        if (command.Contains(messageBlockID))
                            await command.Execute(botClient, update);
                    }
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