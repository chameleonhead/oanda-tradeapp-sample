namespace Rabun.Oanda.Rest.Models
{
public class TransactionMarketOrderCreate: TransactionSimple {
    public float Pl { get; set; }
    public float Interest { get; set; }
    public float AccountBalance { get; set; }
    public TradeOpened TradeOpened { get; set; }
}
}