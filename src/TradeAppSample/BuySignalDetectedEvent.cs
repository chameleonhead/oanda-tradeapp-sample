using System;

namespace TradeAppSample
{
    public class BuySignalDetectedEvent : MarketSignalDetectedEvent<BuySignal>
    {
        public BuySignalDetectedEvent(DateTime timestamp, BuySignal signal) : base(timestamp, signal)
        {
        }
    }
}
