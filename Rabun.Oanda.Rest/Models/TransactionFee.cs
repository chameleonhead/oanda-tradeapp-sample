namespace Rabun.Oanda.Rest.Models
{
    public class TransactionFee: Transaction
    {
        public float Amount;
        public float AccountBalance;
        public OandaTypes.Reason Reason; 
    }
}