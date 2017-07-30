using System;

namespace TradeAppSample
{
    public class Tick
    {
        public Tick(DateTime timestamp, ForexPrice price)
        {
            Timestamp = timestamp;
            Price = price;
        }

        public DateTime Timestamp { get; }
        public ForexPrice Price { get; }

        public static Tick ForNow(decimal ask, decimal bid)
        {
            return new Tick(DateTime.UtcNow, new ForexPrice(ask, bid));
        }
    }
}
