
using AngleSharp;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

using AvaxTelegramBot.Model.DataStructure;

namespace AvaxTelegramBot.Model.Commands
{
    internal class TransactionsCommand : Command
    {
        public override string Name => @"/transactions";
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
                string? blockID = null;

                string message = update.Message.Text;

                await Bot.Commands.First(x => x.Name == "/stop").Execute(botClient, update);
                if (message.Split(" ").Length == 1)
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat, "Для вывода информации о контрактах блока введите blockID");
                    Bot.BlockIDWait = true;
                }
                else
                {
                    blockID = update.Message.Text.Split(" ")[1];

                    using (WebClient wc = new WebClient())
                    {
                        string htmlString = wc.DownloadString(String.Format("https://snowtrace.io/txsInternal?block={0}", blockID));
                        var config = Configuration.Default;
                        var context = BrowsingContext.New(config);
                        var doc = await context.OpenAsync(req => req.Content(htmlString));
                        var tbody = doc.QuerySelector("tbody");
                        var trList = tbody.QuerySelectorAll("tr");
                        if(trList.Length == 1 && trList[0].TextContent == "There are no matching entries")
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat, "Нет контрактов для заданного blockId");
                        }
                        else
                        {
                            Contract contract = new Contract(trList);

                            foreach (string outputString in contract.InformationString())
                                await botClient.SendTextMessageAsync(update.Message.Chat, outputString);
                        }
                    }
                    Bot.BlockIDWait = false;
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
