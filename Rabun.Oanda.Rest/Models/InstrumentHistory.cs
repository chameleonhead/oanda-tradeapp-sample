using System;

namespace Rabun.Oanda.Rest.Models
{
    public class InstrumentHistory
    {
        public String Instrument { get; set; }
        public OandaTypes.GranularityType Granularity { get; set; }
        public CandleMid[] Candles { get; set; }
    }
}
