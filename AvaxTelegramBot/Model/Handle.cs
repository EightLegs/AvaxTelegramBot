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
                await HandleTimer(botClient, update);
            }
            catch(Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                await botClient.SendTextMessageAsync(update.Message.Chat, ex.Message);
            }
            /*if (Timer1 != null && Timer1.Enabled)
            {
                Timer1.Stop();
                Timer1.Dispose();
            }

            else
            {
                Timer1 = new System.Timers.Timer(100000000);
                Timer1.Elapsed += async (sender, e) => await HandleTimer(botClient, update);
                Timer1.Start();
            }*/
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

            string oldLastBlockID = ((Bot)botClient).LastBlockID;
            ((Bot)botClient).LastBlockID = GetLastBlockID(botClient);

            
            List<Block> newBlocks = new List<Block>();
            ulong difference = ulong.Parse(((Bot)botClient).LastBlockID) - ulong.Parse(oldLastBlockID);

            //for(ulong i = ulong.Parse(oldLastBlockID) + 1; i < ulong.Parse(((Bot)botClient).LastBlockID); ++i)

            List<Block> blocks = GetBlocks(botClient, ulong.Parse(oldLastBlockID), ulong.Parse(((Bot)botClient).LastBlockID));

            //await botClient.SendTextMessageAsync(update.Message.Chat, "Новые  " + oldLastBlockID + " " + ((Bot)botClient).LastBlockID);
            //await botClient.SendTextMessageAsync(update.Message.Chat, "Разница " + difference);
            


        }
        //TO DO возможно перенести функции в класс Bot
        private static string GetLastBlockID(ITelegramBotClient botClient)
        {
            string? lastIdBlock = null;
            using (WebClient wc = new WebClient())
            {
                string jsonString = wc.DownloadString(String.Format("https://api.snowtrace.io/api?module=block&action=getblocknobytime&timestamp={0}&closest=before&apikey={1}", DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ((Bot)botClient).ApiKey));
                JObject json = JObject.Parse(jsonString);
                if (json["status"].ToString() == "1")
                {
                    lastIdBlock = json["result"].ToString();
                }
            }
            if (lastIdBlock != null)
                return lastIdBlock;
            else throw new Exception("Couldn`t get last block ID (GetLastBlockID)");
        }
        private static List<Block> GetBlocks(ITelegramBotClient botClient, ulong firstBlock, ulong lastBlock)
        {
            List<Block> blocks = new List<Block>();
            using (WebClient wc = new WebClient())
            {
                for (ulong i = firstBlock; i < lastBlock; ++i)
                {
                    string jsonString = wc.DownloadString(String.Format("https://api.snowtrace.io/api?module=block&action=getblockreward&blockno={0}&apikey={1}", i.ToString(), ((Bot)botClient).ApiKey));
                    JObject json = JObject.Parse(jsonString);
                    //blocks.Add(new Block())
                }
            }


            return blocks;
        }
    }
}


/*
                 string html = wc.DownloadString("https://snowtrace.io/blocks");


                var config = Configuration.Default;
                var context = BrowsingContext.New(config);
                var doc = await context.OpenAsync(req => req.Content(html));

                var element = doc.QuerySelector("td");
 
 */