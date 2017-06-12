namespace Rabun.Oanda.Rest.Models
{
    public class TradeClosed
    {
        public long Id { get; set; }
        public float Price { get; set; }
        public string Instrument { get; set; }
        public float Profit { get; set; }
        public OandaTypes.Side Side { get; set; }
        public string Time { get; set; }
    }
}
