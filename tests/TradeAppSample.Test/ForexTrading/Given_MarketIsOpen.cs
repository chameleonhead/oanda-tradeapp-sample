using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TradeAppSample.ForexTrading;
using TradeAppSample.MarketData;

namespace TradeAppSample.Test.ForexTrading
{
    [TestClass]
    public class Given_MarketIsOpen
    {
        private ForexMarket market;
        private Mock<ITickProvider> tickProvider;
        private Mock<ICandleProvider> candleProvider;
        private DateTime baseTime;

        [TestInitialize]
        public void Setup()
        {
            tickProvider = new Mock<ITickProvider>();
            candleProvider = new Mock<ICandleProvider>();
            baseTime = new DateTime(2017, 1, 2, 3, 4, 5);

            var context = new ForexMarketContext(Instruments.USD_JPY, MarketState.Opened, baseTime);
            market = new ForexMarket(context, tickProvider.Object, candleProvider.Object);
            market.Open();
        }

        [TestMethod]
        public void When_TickProvided_Then_MarketListenerReceiveTickEvent()
        {
            // setup
            var moq = new Mock<IMarketListener>();
            market.Subscribe(moq.Object);

            // when tick provided
            var tick = Tick.ForNow(100, 99);
            tickProvider.Raise(m => m.Tick += null, new TickEventArgs(tick));

            // then market listener should receive tick
            moq.Verify(m => m.OnTick(It.IsAny<Tick>()), Times.Once);
        }

        [TestMethod]
        public void When_CurrentTimeChanged_Then_MarketListenerReceiveMarketContextUpdatedEvent()
        {
            var moq = new Mock<IMarketListener>();
            market.Subscribe(moq.Object);

            // when time changed
            market.SetCurrentTime(baseTime.AddSeconds(5));

            // then market listener should receive tick
            moq.Verify(m => m.OnMarketContextUpdated(It.Is<ForexMarketContext>(c => c.CurrentTime == baseTime.AddSeconds(5))), Times.Once);
        }

        [TestMethod]
        public void When_MarketStateBecomeClosed_Then_MarketStateBecomeClosed()
        {
            Assert.IsTrue(market.CurrentState == MarketState.Opened);
            market.Close();
            Assert.IsTrue(market.CurrentState == MarketState.Closed);
        }
    }
}
