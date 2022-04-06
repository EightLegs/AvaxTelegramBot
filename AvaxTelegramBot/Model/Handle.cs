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
                if (message.Text.StartsWith(@"/"))
                {
                    foreach (var command in Bot.Commands)
                    {
                        if (Bot.BlockIDWait && !message.Text.StartsWith(@"/transactions"))
                        {
                            ulong blockId;
                            bool isblockId = ulong.TryParse(message.Text, out blockId);
                            if (isblockId)
                            {
                                message.Text = String.Format("/transactions {0}", message.Text);
                            }
                            else
                            {
                                botClient.SendTextMessageAsync(update.Message.Chat, "Неверный blockID\n Попробуйте /transactions <blockId>");
                                Bot.BlockIDWait = false;
                            }
                        }
                        if (command.Contains(message))
                            await command.Execute(botClient, update);
                    }
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}