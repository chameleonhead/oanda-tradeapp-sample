using System;

namespace Rabun.Oanda.Rest.Models
{
    public class TransactionOrderUpdate : Transaction
    {
        public int Units { get; set; }
        public float Price { get; set; }
        public OandaTypes.Reason Reason { get; set; }
        public DateTime? Expiry { get; set; }
    }
}
