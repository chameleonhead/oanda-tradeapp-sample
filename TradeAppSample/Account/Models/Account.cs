// Copied From: https://github.com/rabun/oanda-rest-cs
namespace Rabun.Oanda.Rest.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public float MarginRate { get; set; }
        public AccountCurrency AccountCurrency { get; set; }
    }
}