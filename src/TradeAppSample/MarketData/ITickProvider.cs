using System;

namespace TradeAppSample.MarketData
{
    public interface ITickProvider
    {
        event EventHandler<TickEventArgs> Tick;
    }
}
