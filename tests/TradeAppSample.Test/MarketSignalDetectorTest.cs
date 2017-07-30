using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TradeAppSample.Test
{
    class TestMarketSignalDetector : MarketSignalDetectorBase
    {
        public void RaiseBuySignal()
        {
            OnBuySignalDetected(new BuySignal());
        }
        public void RaiseSellSignal()
        {
            OnSellSignalDetected(new SellSignal());
        }
    }

    [TestClass]
    public class MarketSignalDetectorTest
    {
        [TestMethod]
        public void ShouldRaiseBuySignalDetectedEventWhenProtectedWhenBuySignalDetected()
        {
            var moq = new Mock<IEventHandler<BuySignalDetectedEvent>>();
            moq.Setup(m => m.Handle(It.IsAny<BuySignalDetectedEvent>()));
            var signalDetector = new TestMarketSignalDetector();
            signalDetector.RaiseBuySignal();
            moq.VerifyAll();
        }

        [TestMethod]
        public void ShouldRaiseSellSignalDetectedEventWhenProtectedWhenSellSignalDetected()
        {
            var moq = new Mock<IEventHandler<SellSignalDetectedEvent>>();
            moq.Setup(m => m.Handle(It.IsAny<SellSignalDetectedEvent>()));
            var signalDetector = new TestMarketSignalDetector();
            signalDetector.RaiseBuySignal();
            moq.VerifyAll();
        }
    }
}
