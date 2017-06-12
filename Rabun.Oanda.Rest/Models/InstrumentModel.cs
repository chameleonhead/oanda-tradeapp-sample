using System;

namespace Rabun.Oanda.Rest.Models
{
    public class InstrumentModel
    {
        public string Instrument { get; set; }
        public string DisplayName { get; set; }
        public float Pip { get; set; }
        public int PaxTradeUnits { get; set; }
    }
}