namespace Rabun.Oanda.Rest.Models
{
    public class TradeOpened
    {
        public long Id { get; set; }
        public int Units { get; set; }
        public float TakeProfit { get; set; }
        public float StopLoss { get; set; }
        public float TrailingStop { get; set; }
        public OandaTypes.Side Side { get; set; }
    }
}
