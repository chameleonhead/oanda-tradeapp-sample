using System;
using TradeAppSample.ForexTrading;

namespace TradeAppSample.TradingStrategies.MarketSignalDetection
{
    public class SellSignalDetectedEvent : MarketSignalDetectedEvent<SellSignal>
    {
        public SellSignalDetectedEvent(DateTime timestamp, SellSignal signal) : base(timestamp, signal)
        {
        }
    }
}
