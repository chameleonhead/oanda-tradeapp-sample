using System;

namespace Rabun.Oanda.Rest.Models
{
    public class InstrumentModel
    {
        public string Instrument { get; set; }
        public string DisplayName { get; set; }
        public float Pip { get; set; }
        public int MaxTradeUnits { get; set; }
        public float Precision { get; set; }
        public float MaxTrailingStop { get; set; }
        public float MinTrailingStop { get; set; }
        public float MarginRate { get; set; }
        public bool Halted { get; set; }
    }
}