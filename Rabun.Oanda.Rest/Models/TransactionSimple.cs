
namespace Rabun.Oanda.Rest.Models
{
    public class TransactionSimple : Transaction
    {
        public string Instrument { get; set; }
        public int Units { get; set; }
        public OandaTypes.Side Side { get; set; }
        public float Price { get; set; }
    }
}
