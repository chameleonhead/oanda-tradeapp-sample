using System;
using TradeAppSample.ForexTrading;

namespace TradeAppSample.TradingStrategies.MarketSignalDetection
{
    public class BuySignalDetectedEvent : MarketSignalDetectedEvent<BuySignal>
    {
        public BuySignalDetectedEvent(DateTime timestamp, BuySignal signal) : base(timestamp, signal)
        {
        }
    }
}
