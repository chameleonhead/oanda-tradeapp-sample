using System;

namespace TradeAppSample
{
    public class SellSignalDetectedEvent : MarketSignalDetectedEvent<SellSignal>
    {
        public SellSignalDetectedEvent(DateTime timestamp, SellSignal signal) : base(timestamp, signal)
        {
        }
    }
}
