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


using AngleSharp;
using Newtonsoft.Json.Linq;

namespace AvaxTelegramBot.Model
{
    public static class Handle
    {
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
            try 
            {
                if (Timer1 != null && Timer1.Enabled)
                {
                    Timer1.Stop();
                    Timer1.Dispose();
                }

                else
                {
                    Timer1 = new System.Timers.Timer(5000);
                    Timer1.Elapsed += async (sender, e) => await HandleTimer(botClient, update);
                    Timer1.Start();
                }

               //await HandleTimer(botClient, update);
            }
            catch(Exception ex)
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


            ulong oldBlockId = (ulong)((Bot)botClient).LastBlockID;
            ((Bot)botClient).LastBlockID = GetLastBlockID(botClient);
            ulong lastBlockId = (ulong)((Bot)botClient).LastBlockID;

            //
            ulong difference = lastBlockId - oldBlockId;

            botClient.SendTextMessageAsync(update.Message.Chat, String.Format("{0} {1}", lastBlockId, oldBlockId));
            //


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

        //TO DO парсинг контрактов по блоку https://snowtrace.io/txsInternal
        //TO DO возможно перенести функции в класс Bot
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