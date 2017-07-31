using System;

namespace TradeAppSample.Shared
{
    public class EventBase : IEvent
    {
        protected EventBase(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp { get; }
    }
}
