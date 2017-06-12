using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rabun.Oanda.Rest.Models
{
    public class OrderOpened
    {
        public long Id { get; set; }
        public int Units { get; set; }
        public float TakeProfit { get; set; }
        public float StopLoss { get; set; }
        public float TrailingStop { get; set; }
        public OandaTypes.Side Side { get; set; }
        public string Expiry { get; set; }
        public float UpperBound { get; set; }
        public float LowerBound { get; set; }
    }
}
