using System;

namespace TradeAppSample.ForexTrading
{
    public class SellSignal : MarketSignal
    {
        public SellSignal(Tick tick) : base(tick)
        {
        }

        public SellSignal(DateTime timestamp, Tick tick) : base(timestamp, tick)
        {
        }
    }
}
