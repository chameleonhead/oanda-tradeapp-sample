namespace Rabun.Oanda.Rest.Models
{
    public class Trade
    {
        public long Id { get; set; }
        public int Units { get; set; }
        public OandaTypes.Side Side { get; set; }
        public string Instrument { get; set; }
        public string Time { get; set; }
        public float Price { get; set; }
        public float TakeProfit { get; set; }
        public float StopLoss { get; set; }
        public float TrailingStop { get; set; }
        public float TrailingAmount { get; set; }
    }
}
