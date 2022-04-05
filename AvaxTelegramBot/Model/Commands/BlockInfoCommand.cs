
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AvaxTelegramBot.Model.Commands
{
    internal class BlockInfoCommand : Command
    {
        public override string Name => @"/blockinfo";
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
                ulong? blockID = null;

                string message = update.Message.Text;

                if(message.Split(" ").Length == 1)
                {
                    await Bot.Commands.First(x => x.Name == "/stop").Execute(botClient, update);
                    await botClient.SendTextMessageAsync(update.Message.Chat, "Для вывода информации о контрактах блока введите blockID");
                    Bot.BlockIDWait = true;
                }
                else
                {
                    blockID = ulong.Parse(update.Message.Text.Split(" ")[1]);

                    string outputMessage = blockContract(blockID.Value);




                    Bot.BlockIDWait = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                await botClient.SendTextMessageAsync(update.Message.Chat, ex.Message);
            }
        }

        private string blockContract(ulong blockID)
        {
            string result = null;



            return result;
        }
    }
}
