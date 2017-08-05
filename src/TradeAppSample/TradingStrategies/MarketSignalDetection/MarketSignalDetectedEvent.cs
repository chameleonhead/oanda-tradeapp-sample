using System;
using TradeAppSample.ForexTrading;
using TradeAppSample.Shared;

namespace TradeAppSample.TradingStrategies.MarketSignalDetection
{
    public abstract class MarketSignalDetectedEvent<T> : EventBase
        where T: MarketSignal
    {
        public MarketSignalDetectedEvent(DateTime timestamp, T signal) : base(timestamp)
        {
            Signal = signal;
        }

        public virtual T Signal { get; }
    }
}
