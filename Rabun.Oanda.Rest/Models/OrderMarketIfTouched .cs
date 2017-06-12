using System;

namespace Rabun.Oanda.Rest.Models
{
    public class OrderMarketIfTouched: Order 
    {
        public int Units;
        public float TakeProfit { get; set; }
        public float StopLoss { get; set; }
        public string Expiry { get; set; }
        public float UpperBound { get; set; }
        public float LowerBound { get; set; }
        public float TrailingStop { get; set; }
    }
}