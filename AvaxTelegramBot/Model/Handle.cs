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
            await HandleTimer(botClient, update);
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
            using (WebClient wc = new WebClient())
            {

                if (((Bot)botClient).LastBlockID == null)
                {
                    string jsonString = wc.DownloadString(String.Format("https://api.snowtrace.io/api?module=block&action=getblocknobytime&timestamp={0}&closest=before&apikey={1}", DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ((Bot)botClient).ApiKey));
                    JObject json = JObject.Parse(jsonString);
                    if(json["status"].ToString() == "1")
                        ((Bot)botClient).LastBlockID = json["result"].ToString();
                    int b = 5;
                }
                else
                {

                }
            }
            await botClient.SendTextMessageAsync(update.Message.Chat, "Привет-привет!!");
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