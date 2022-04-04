using Telegram.Bot;
using Telegram.Bot.Types;

namespace AvaxTelegramBot.Model.Commands
{
    internal class StopCommand : Command
    {
        public override string Name => @"/stop";
        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public override async Task Execute(ITelegramBotClient botClient, Update update)
        {
            try
            {
                if (Bot.Timer1 != null && Bot.Timer1.Enabled)
                {
                    Bot.Timer1.Stop();
                    Bot.Timer1.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                await botClient.SendTextMessageAsync(update.Message.Chat, ex.Message);
            }
        }
    }
}
