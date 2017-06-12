namespace Rabun.Oanda.Rest.Models
{
    public class AccountDetails
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public float Balance { get; set; }
        public float UnrealizedPl { get; set; }
        public float RealizedPl { get; set; }
        public float MarginUsed { get; set; }
        public float MarginAvail { get; set; }
        public float MarginRate { get; set; }
        public int OpenTrades { get; set; }
        public int OpenOrders { get; set; }
        public OandaTypes.AccountCurrency AccountCurrency { get; set; }
    }
}
