using System;
using TradeAppSample.Shared;

namespace TradeAppSample.ForexTrading.MarketSignalDetection
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
