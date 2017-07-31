using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradeAppSample.ForexTrading;
using TradeAppSample.ForexTrading.MarketSignalDetection;
using TradeAppSample.Shared;
using TradeAppSample.ForexTrading.TradingStrategies.SwingLowHigh;

namespace TradeAppSample.Test.ForexTrading.TradingStrategies.SwingLowHigh
{
    [TestClass]
    public class SwingLowHighSignalDetectorTest
    {
        [TestMethod]
        public void ShouldRaiseBuySignalIfTickAskPriceOvercomeContextsSwingHighAskPrice()
        {
            var moq = new Mock<IEventHandler<BuySignalDetectedEvent>>();
            var context = new SwingLowHighContext(new ForexPrice(100, 99), new ForexPrice(90, 89));
            var detector = new SwingLowHighSignalDetector();
            detector.DetectSignal(context, Tick.ForNow(100, 99));
            detector.DetectSignal(context, Tick.ForNow(101, 100));
            moq.Verify(m => m.Handle(It.IsAny<BuySignalDetectedEvent>()), Times.Once);
        }

        [TestMethod]
        public void ShouldRaiseSellSignalIfTickBidPriceDownUnderContextsSwingLowAskPrice()
        {
            var moq = new Mock<IEventHandler<BuySignalDetectedEvent>>();
            var context = new SwingLowHighContext(new ForexPrice(100, 99), new ForexPrice(91, 90));
            var detector = new SwingLowHighSignalDetector();
            detector.DetectSignal(context, Tick.ForNow(91, 90));
            detector.DetectSignal(context, Tick.ForNow(90, 89));
            moq.Verify(m => m.Handle(It.IsAny<BuySignalDetectedEvent>()), Times.Once);
        }
    }
}
