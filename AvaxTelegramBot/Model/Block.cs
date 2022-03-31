using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using AngleSharp;

namespace AvaxTelegramBot.Model
{
    public class Block
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Txn { get; set; }
        public string Hash { get; set; }
        public string GasUsed { get; set; }
        public string GasLimit { get; set; }
        public string BurnedAvax { get; set; }

        public Block(AngleSharp.Dom.IElement element)
        {

        }
    }
}
