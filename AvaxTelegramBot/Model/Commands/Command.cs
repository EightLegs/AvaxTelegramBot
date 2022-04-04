using Telegram.Bot;
using Telegram.Bot.Types;

namespace AvaxTelegramBot.Model.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract Task Execute(ITelegramBotClient botClient, Update update);

        public abstract bool Contains(Message message);
    }
}
