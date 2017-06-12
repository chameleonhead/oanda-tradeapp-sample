namespace Rabun.Oanda.Rest.Models
{
    public class CandleMid
    {
        public string time { get; set; }
        public float openMid { get; set; }
        public float highMid { get; set; }
        public float lowMid { get; set; }
        public float closeMid { get; set; }
        public int volume { get; set; }
        public bool complete { get; set; }
    }
}

