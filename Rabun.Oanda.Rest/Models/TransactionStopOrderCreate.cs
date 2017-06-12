using System;

namespace Rabun.Oanda.Rest.Models
{
    public class TransactionStopOrderCreate : TransactionSimple
    {
        public DateTime Expiry { get; set; }
        public OandaTypes.Reason Reason { get; set; }
    }
}