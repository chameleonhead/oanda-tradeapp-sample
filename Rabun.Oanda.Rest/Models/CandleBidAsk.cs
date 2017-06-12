using System;

namespace Rabun.Oanda.Rest.Models
{
    public class CandleBidAsk
    {
        public string Time { get; set; }
        public float OpenBid { get; set; }
        public float OpenAsk { get; set; }
        public float HighBid { get; set; }
        public float HighAsk { get; set; }
        public float LowBid { get; set; }
        public float LowAsk { get; set; }
        public float CloseBid { get; set; }
        public float CloseAsk { get; set; }
        public int Volume { get; set; }
        public bool Complete { get; set; }
    }
}
