using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaxTelegramBot.Model.DataStructure
{
    public class Transaction
    {
        public string ParentTxnHash { get; set; }
        public string Type { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Value { get; set; }

        public Transaction(AngleSharp.Dom.IElement element)
        {
            int mainFieldOffset = 6;
            int columnOffset= element.Children.Length - mainFieldOffset;
            ParentTxnHash = element.Children[columnOffset].TextContent;
            Type = element.Children[columnOffset + 1].TextContent;
            FromAddress = element.Children[columnOffset + 2].TextContent;
            ToAddress = element.Children[columnOffset + 4 ].TextContent;
            Value = element.Children[columnOffset + 5].TextContent.Split(" ")[0].Replace(",", ".");
        }

        public string InformationString()
        {
            return String.Format("Parent Txn Hash : {0}\nType : {1}\nFrom : {2}\nTo : {3}\nValue : {4} Avax", ParentTxnHash,Type, FromAddress, ToAddress, Value);
        }
    }
}
