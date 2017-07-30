using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradeAppSample.Test
{
    [TestClass]
    public class SwingLowHighContextFactoryTest
    {
        [TestMethod]
        public void ShouldRecognizeSwingHighAndSwingLowFromPassedCandles()
        {
            var candles = new ForexCandle[] {
                ForexCandleUtils.Create(100, 110, 90, 101),
                ForexCandleUtils.Create(101, 102, 90, 90),
                ForexCandleUtils.Create(90, 111, 89, 110),
                ForexCandleUtils.Create(110, 120, 100, 115),
                ForexCandleUtils.Create(115, 117, 113, 110),
            };
            var factory = new SwingLowHighContextFactory();
            var context = factory.Create(candles);
            Assert.AreEqual(120, context.SwingHigh.Bid);
            Assert.AreEqual(89, context.SwingLow.Bid);
        }
    }
}
