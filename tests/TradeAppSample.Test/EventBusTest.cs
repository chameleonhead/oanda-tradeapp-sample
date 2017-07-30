using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace TradeAppSample.Test
{
    public class TestEvent1 : EventBase
    {
        public TestEvent1() : base(DateTime.UtcNow)
        {
        }
    }

    public class TestEvent2 : EventBase
    {
        public TestEvent2() : base(DateTime.UtcNow)
        {
        }
    }

    [TestClass]
    public class EventBusTest
    {
        [TestMethod]
        public void ShouldCallEventHandlerWhenEventHandlerIsRegisterdAndEventRaised()
        {
            var moq = new Mock<IEventHandler<TestEvent1>>();
            var eventHandler = moq.Object;
            var evt = new TestEvent1();
            EventBus.Subscribe(eventHandler);
            EventBus.Publish(evt);
            moq.Verify(o => o.Handle(evt), Times.Once);

            EventBus.Unsubscribe(eventHandler);
        }

        [TestMethod]
        public void ShouldCallEventHandlerTwiceWhenMultipleEventHandlersAreRegisterdAndEventRaised()
        {
            var moq = new Mock<IEventHandler<TestEvent1>>();
            var eventHandler1 = moq.Object;
            var eventHandler2 = moq.Object;
            var evt = new TestEvent1();
            EventBus.Subscribe(eventHandler1);
            EventBus.Subscribe(eventHandler2);

            EventBus.Publish(evt);
            moq.Verify(o => o.Handle(evt), Times.Exactly(2));

            EventBus.Unsubscribe(eventHandler1);
            EventBus.Unsubscribe(eventHandler2);
        }

        [TestMethod]
        public void ShouldCallEventHandlerOnceWhenMultipleEventHandlersWhichHandleDifferentEventsAreRegisterdAndEventRaised()
        {
            var moq1 = new Mock<IEventHandler<TestEvent1>>();
            var moq2 = new Mock<IEventHandler<TestEvent2>>();
            var eventHandler1 = moq1.Object;
            var eventHandler2 = moq2.Object;
            var evt = new TestEvent1();
            EventBus.Subscribe(eventHandler1);
            EventBus.Subscribe(eventHandler2);

            EventBus.Publish(evt);
            moq1.Verify(o => o.Handle(evt), Times.Once);
            moq2.Verify(o => o.Handle(null), Times.Never);

            EventBus.Unsubscribe(eventHandler1);
            EventBus.Unsubscribe(eventHandler2);
        }
    }
}
