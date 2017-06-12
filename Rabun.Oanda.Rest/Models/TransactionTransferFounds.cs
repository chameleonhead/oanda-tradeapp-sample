namespace Rabun.Oanda.Rest.Models
{
    public class TransactionTransferFounds : Transaction
    {
        public float Amount { get; set; }
        public float AccountBalance { get; set; }
        public OandaTypes.Reason Reason { get; set; }
    }
}
