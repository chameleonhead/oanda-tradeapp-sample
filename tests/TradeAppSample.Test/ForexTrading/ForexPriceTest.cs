using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeAppSample.ForexTrading;

namespace TradeAppSample.Test.ForexTrading
{
    [TestClass]
    public class ForexPriceTest
    {
        [TestMethod]
        public void ShouldReturnZeroIfOtherHasSameValue()
        {
            var value1 = new ForexPrice(100, 99);
            var value2 = new ForexPrice(100, 99);
            Assert.IsTrue(value1.CompareTo(value2) == 0);
            Assert.IsTrue(value1 == value2);
        }

        [TestMethod]
        public void ShouldReturnPositiveIfOtherHasLessValue()
        {
            var value1 = new ForexPrice(100, 99);
            var value2 = new ForexPrice(99, 98);
            Assert.IsTrue(value1.CompareTo(value2) > 0);
            Assert.IsTrue(value1 != value2);
            Assert.IsTrue(value1 > value2);
        }

        [TestMethod]
        public void ShouldReturnNegativeIfOtherHasGreaterValue()
        {
            var value1 = new ForexPrice(100, 99);
            var value2 = new ForexPrice(101, 100);
            Assert.IsTrue(value1.CompareTo(value2) < 0);
            Assert.IsTrue(value1 != value2);
            Assert.IsTrue(value1 < value2);
        }
    }
}
