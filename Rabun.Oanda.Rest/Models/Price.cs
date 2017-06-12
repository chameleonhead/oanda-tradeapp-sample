namespace Rabun.Oanda.Rest.Models
{
    public class Price
    {
        public string Instrument { get; set; }
        public string Time { get; set; }
        public float Bid { get; set; }
        public float Ask { get; set; }
        public string Status { get; set; }
    }
}