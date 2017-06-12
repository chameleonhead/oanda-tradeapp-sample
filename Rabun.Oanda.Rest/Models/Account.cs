namespace Rabun.Oanda.Rest.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public float MarginRate { get; set; }
        public OandaTypes.AccountCurrency AccountCurrency { get; set; }
    }
}

