using Telegram.Bot;
using Telegram.Bot.Types;
using AngleSharp;
using System.Net;
using Newtonsoft.Json.Linq;


namespace AvaxTelegramBot.Model.Commands
{
    internal class StartCommand : Command
    {
        public override string Name => @"/start";
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
                if (Bot.Timer1 == null)
                {
                    Bot.Timer1 = new System.Timers.Timer(5000);
                    Bot.Timer1.Elapsed += async (sender, e) => await HandleTimer(botClient, update);
                    Bot.Timer1.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                await botClient.SendTextMessageAsync(update.Message.Chat, ex.Message);
            }
        }

        private static async Task HandleTimer(ITelegramBotClient botClient, Update update)
        {
            //TO DO проверить переполнение ID блока
            if (((Bot)botClient).LastBlockID == null)
            {
                ((Bot)botClient).LastBlockID = GetLastBlockID(botClient);
                await botClient.SendTextMessageAsync(update.Message.Chat, "Установлен блок с ID " + ((Bot)botClient).LastBlockID);
                return;
            }

            ulong oldBlockId = (ulong)((Bot)botClient).LastBlockID != 0 ? (ulong)((Bot)botClient).LastBlockID : GetLastBlockID(botClient);
            ((Bot)botClient).LastBlockID = GetLastBlockID(botClient);
            ulong lastBlockId = (ulong)((Bot)botClient).LastBlockID;

            botClient.SendTextMessageAsync(update.Message.Chat, String.Format("{0} - {1}", oldBlockId, lastBlockId));

            try 
            {
                List<Block> newBlocks = new List<Block>();

                using (WebClient wc = new WebClient())
                {
                    string htmlString = wc.DownloadString("https://snowtrace.io/blocks");
                    var config = Configuration.Default;
                    var context = BrowsingContext.New(config);
                    var doc = await context.OpenAsync(req => req.Content(htmlString));
                    var tbody = doc.QuerySelector("tbody");
                    var trList = tbody.QuerySelectorAll("tr");

                    foreach (var tr in trList)
                    {
                        if (tr.Children.Length == Block.ParseFieldCount)
                        {
                            Block block = new Block(tr);

                            if (block.Id < lastBlockId && block.Id >= oldBlockId)
                                newBlocks.Add(block);
                        }
                    }
                }

                newBlocks.Sort();
                foreach (Block block in newBlocks)
                    botClient.SendTextMessageAsync(update.Message.Chat, block.InformationString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                await botClient.SendTextMessageAsync(update.Message.Chat, ex.Message);
            }
        }
        private static ulong GetLastBlockID(ITelegramBotClient botClient)
        {
            ulong? lastIdBlock = null;
            using (WebClient wc = new WebClient())
            {
                string jsonString = wc.DownloadString(String.Format("https://api.snowtrace.io/api?module=block&action=getblocknobytime&timestamp={0}&closest=before&apikey={1}", DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ((Bot)botClient).ApiKey));
                JObject json = JObject.Parse(jsonString);
                if (json["status"].ToString() == "1")
                {
                    lastIdBlock = ulong.Parse(json["result"].ToString());
                }
            }
            if (lastIdBlock != null)
                return (ulong)lastIdBlock;
            else throw new Exception("Couldn`t get last block ID (GetLastBlockID)");
        }

    }
}