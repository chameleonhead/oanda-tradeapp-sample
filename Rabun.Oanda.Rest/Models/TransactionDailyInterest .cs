namespace Rabun.Oanda.Rest.Models
{
    public class TransactionDailyInterest : Transaction
    {
        public string Instrument { get; set; }
        public float Interest { get; set; }
        public float AccountBalance { get; set; }
    }
}