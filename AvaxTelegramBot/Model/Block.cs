using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using AngleSharp;

namespace AvaxTelegramBot.Model
{
    public class Block : IComparable
    {
        public ulong Id { get; set; }
        public DateTime DateTime { get; set; }
        public string TimeAgo { get; set; }
        public string Txn { get; set; }
        public string Hash { get; set; }
        public string GasUsed { get; set; }
        public string GasLimit { get; set; }
        public string BurnedAvax { get; set; }

        public static int ParseFieldCount { get; set; } = 8;
        /*
        public override bool Equals(object? obj)
        {
            return Equals(obj as Block);
        }

        public bool Equals(Block? other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public static bool operator >(Block block1, Block block2)
        {
            return block1.Id > block2.Id;
        }
        public static bool operator <(Block block1, Block block2)
        {
            return block1.Id < block2.Id;
        }
        */
        public Block(AngleSharp.Dom.IElement element)
        {
            Id = ulong.Parse(element.Children[0].TextContent);
            DateTime = DateTime.Parse(element.Children[1].TextContent);
            TimeAgo = element.Children[2].TextContent;
            Txn = element.Children[3].TextContent;
            Hash = element.Children[4].TextContent;
            GasUsed = element.Children[5].TextContent;
            GasLimit = element.Children[6].TextContent;
            BurnedAvax = element.Children[7].TextContent;
        }

        public string InformationString()
        {
            return String.Format("Block with id {0}\nAdded {1} ({2})\nTxn : {3}\nHash : {4}\nGasUed : {5}\nGasLimit : {6}\nAvaxBurned : {7}", Id.ToString(),DateTime.ToString(),TimeAgo,Txn,Hash,GasUsed,GasLimit,BurnedAvax);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) throw new ArgumentException("Некорректное значение параметра");

            Block otherBlock = obj as Block;
            return this.Id.CompareTo(otherBlock.Id);
        }
    }
}
