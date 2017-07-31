using System;

namespace TradeAppSample.ForexTrading.MarketSignalDetection
{
    public class SellSignalDetectedEvent : MarketSignalDetectedEvent<SellSignal>
    {
        public SellSignalDetectedEvent(DateTime timestamp, SellSignal signal) : base(timestamp, signal)
        {
        }
    }
}
