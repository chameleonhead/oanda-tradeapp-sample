using System;
using TradeAppSample.ForexTrading;

namespace TradeAppSample.MarketData
{
    public class TickEventArgs: EventArgs
    {
        public TickEventArgs(Tick tick)
        {
            Tick = tick;
        }
        
        public Tick Tick { get; }
    }
}