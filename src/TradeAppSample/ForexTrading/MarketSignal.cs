using System;

namespace TradeAppSample.ForexTrading
{
    public abstract class MarketSignal
    {
        public MarketSignal(DateTime timestamp, Tick tick)
        {
            Timestamp = timestamp;
            Tick = tick;
        }

        public MarketSignal(Tick tick) : this(DateTime.UtcNow, tick)
        {
        }

        public DateTime Timestamp { get; }
        public Tick Tick { get; }
    }
}