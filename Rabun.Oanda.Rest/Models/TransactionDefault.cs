using System;

namespace Rabun.Oanda.Rest.Models
{
    public class TransactionDefault : TransactionSimple
    {
        public DateTime? Expiry { get; set; }
        public OandaTypes.Reason Reason { get; set; }
        public float? LowerBound { get; set; }
        public float? UpperBound { get; set; }
        public float? TakeProfitPrice { get; set; }
        public float? StopLossPrice { get; set; }
        public float? TrailingStopLossDistance { get; set; }
    }
}