using System;

namespace TradeAppSample
{
    public interface IEvent
    {
        DateTime Timestamp { get; }
    }
}