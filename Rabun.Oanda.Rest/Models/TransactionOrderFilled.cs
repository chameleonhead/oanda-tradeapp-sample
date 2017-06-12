namespace Rabun.Oanda.Rest.Models
{
    public class TransactionOrderFilled : TransactionDefault
    {
        public long OrderId;
        public int Pl;
        public int Interest;
        public TradeOpened TradeOpened;
    }
}