using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace TradeAppSample.Test
{
    [TestClass]
    public class SwingLowHighPointCalculatorTest
    {
        [TestMethod]
        public void ShouldRecognizeSwingHighPointAndSwingLowPointFromPassedLine()
        {
            var candles = new ForexCandle[] {
                ForexCandleUtils.Create(100, 110, 90, 101),
                ForexCandleUtils.Create(101, 102, 90, 90),
                ForexCandleUtils.Create(90, 111, 89, 110),
                ForexCandleUtils.Create(110, 120, 100, 115),
                ForexCandleUtils.Create(115, 117, 113, 110),
            };
            var calculator = new SwingLowHighPointCalculator();
            var result = calculator.Calculate(candles);
            Assert.AreEqual(120, result.High.Bid);
            Assert.AreEqual(89, result.Low.Bid);
        }
    }
}
