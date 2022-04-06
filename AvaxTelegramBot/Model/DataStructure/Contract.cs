using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaxTelegramBot.Model.DataStructure
{
    public class Contract
    {
        public ulong Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Age { get; set; }
        private  List<Transaction> TransactionList;
        public  IReadOnlyList<Transaction> Transactions => TransactionList.AsReadOnly();
        public Contract(AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> element)
        {
            TransactionList = new List<Transaction>();

            Id = ulong.Parse(element[0].Children[0].TextContent);
            DateTime = DateTime.Parse(element[0].Children[1].TextContent);
            Age = element[0].Children[2].TextContent;

            foreach(var td in element)
            {
                Transaction transaction = new Transaction(td);
                TransactionList.Add(transaction);
            }
        }

        public string[] InformationString()
        {
            string[] result = new string[Transactions.Count + 1];
            result[0] = String.Format("Block with id {0}\nAdded {1} ({2})\nHas {3} contracts:", Id.ToString(), DateTime.ToString(), Age, Transactions.Count);
            for(int i = 1; i <= Transactions.Count; i++)
            {
                result[i] = Transactions[i-1].InformationString();
            }
            return result;
        }
    }
}
