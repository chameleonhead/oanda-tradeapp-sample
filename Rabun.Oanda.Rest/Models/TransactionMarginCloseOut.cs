namespace Rabun.Oanda.Rest.Models
{
    public class TransactionMarginCloseOut : TransactionSimple
    {
        public long TradeId;
        public float Pl;
        public int Interest;
        public float AccountBalance;
    }
}