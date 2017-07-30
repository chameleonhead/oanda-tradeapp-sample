using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeAppSample
{
    public class EventBus
    {
        static Dictionary<Type, List<object>> eventHandlers = new Dictionary<Type, List<object>>();

        public static void Publish<T>(T evt) where T : IEvent
        {
            if (eventHandlers.TryGetValue(typeof(IEventHandler<T>), out var handlers))
            {
                handlers.Cast<IEventHandler<T>>().ToList().ForEach(o => o.Handle(evt));
            }
        }

        public static void Subscribe<T>(IEventHandler<T> eventHandler) where T : IEvent
        {
            if (!eventHandlers.TryGetValue(typeof(IEventHandler<T>), out var handlers))
            {
                handlers = new List<object>();
                eventHandlers.Add(typeof(IEventHandler<T>), handlers);
            }
            handlers.Add(eventHandler);
        }

        public static void Unsubscribe<T>(IEventHandler<T> eventHandler) where T : IEvent
        {
            if (eventHandlers.TryGetValue(typeof(IEventHandler<T>), out var handlers))
            {
                handlers.Remove(eventHandler);
                if (!handlers.Any())
                {
                    eventHandlers.Remove(typeof(IEventHandler<T>));
                }
            }
        }
    }
}
