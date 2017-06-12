using System;

namespace Rabun.Oanda.Rest.Models
{
    public class Order
    {
        public long Id { get; set; }
        public string Instrument { get; set; }
        public string Time { get; set; }
        public float Price { get; set; }
        public OandaTypes.OrderType Type { get; set; }
        public OandaTypes.Side Side { get; set; }
    }
}