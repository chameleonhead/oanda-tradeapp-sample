using System;

namespace TradeAppSample.ForexTrading
{
    public class BuySignal : MarketSignal
    {
        public BuySignal(Tick tick) : base(tick)
        {
        }

        public BuySignal(DateTime timestamp, Tick tick) : base(timestamp, tick)
        {
        }
    }
}
