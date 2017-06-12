namespace Rabun.Oanda.Rest.Models
{
    public class TransactionTradeUpdate : Transaction
    {
        public long TradeId { get; set; }
        public int Units { get; set; }
        public OandaTypes.Side Side { get; set; }
        public float StopLossPrice { get; set; }
    }
}