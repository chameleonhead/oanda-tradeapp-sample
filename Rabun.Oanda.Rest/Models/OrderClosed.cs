namespace Rabun.Oanda.Rest.Models
{
    public class OrderClosed
    {
        public long Id { get; set; }
        public string Instrument { get; set; }
        public string Time { get; set; }
        public float Price { get; set; }
        public OandaTypes.Side Side { get; set; }
        public int Units { get; set; }
    }
}
