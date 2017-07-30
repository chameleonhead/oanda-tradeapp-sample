using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TradeAppSample.Test
{
    [TestClass]
    public class SwingLowHighDetectorTest
    {
        [TestMethod]
        public void ShouldRaiseBuySignalIfTickAskPriceOvercomeContextsSwingHighAskPrice()
        {
            var moq = new Mock<IEventHandler<BuySignalDetectedEvent>>();
            var context = new SwingLowHighContext(100, 90);
            var detector = new SwingLowHighDetector();
            detector.DetectSignal(context, Tick.ForNow(Instruments.USD_JPY, 100, 99));
            detector.DetectSignal(context, Tick.ForNow(Instruments.USD_JPY, 101, 100));
            moq.Verify(m => m.Handle(It.IsAny<BuySignalDetectedEvent>()), Times.Once);
        }

        [TestMethod]
        public void ShouldRaiseSellSignalIfTickBidPriceDownUnderContextsSwingLowAskPrice()
        {
            var moq = new Mock<IEventHandler<SellSignalDetectedEvent>>();
            var context = new SwingLowHighContext(100, 90);
            var detector = new SwingLowHighDetector();
            detector.DetectSignal(context, Tick.ForNow(Instruments.USD_JPY, 91, 90));
            detector.DetectSignal(context, Tick.ForNow(Instruments.USD_JPY, 90, 89));
            moq.Verify(m => m.Handle(It.IsAny<SellSignalDetectedEvent>()), Times.Once);
        }
    }
}
