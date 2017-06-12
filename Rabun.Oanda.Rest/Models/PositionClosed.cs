using System;

namespace Rabun.Oanda.Rest.Models
{
    public class PositionClosed
    {
        public long[] Ids { get; set; }
        public string Instrument { get; set; }
        public int TotalUnits { get; set; }
        public float Price { get; set; }
    }
}