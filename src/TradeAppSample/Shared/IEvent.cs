using System;

namespace TradeAppSample.Shared
{
    public interface IEvent
    {
        DateTime Timestamp { get; }
    }
}