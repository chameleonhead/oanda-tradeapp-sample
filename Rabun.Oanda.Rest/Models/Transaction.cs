using System;

namespace Rabun.Oanda.Rest.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public int AccountId { get; set; }
        public string Time { get; set; }
        public OandaTypes.TransactionType Type { get; set; }
    }
}