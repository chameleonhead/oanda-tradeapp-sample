using System;

namespace TradeAppSample
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
