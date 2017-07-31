using System;

namespace TradeAppSample.ForexTrading.MarketSignalDetection
{
    public class BuySignalDetectedEvent : MarketSignalDetectedEvent<BuySignal>
    {
        public BuySignalDetectedEvent(DateTime timestamp, BuySignal signal) : base(timestamp, signal)
        {
        }
    }
}
