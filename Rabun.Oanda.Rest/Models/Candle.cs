using System.Collections.Generic;

namespace Rabun.Oanda.Rest.Models
{
    public class Candle<T>
    {
        public Candle()
        {
            Candles = new List<T>();
        } 

        public string Instrument { get; set; }
        public OandaTypes.GranularityType Granularity { get; set; }
        public List<T> Candles { get; set; }
    }
}
