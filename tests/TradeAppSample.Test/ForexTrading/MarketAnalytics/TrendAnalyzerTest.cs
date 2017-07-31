using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeAppSample.ForexTrading;
using TradeAppSample.ForexTrading.MarketAnalytics;

namespace TradeAppSample.Test.ForexTrading.MarketAnalytics
{
    [TestClass]
    public class TrendAnalyzerTest
    {
        [TestMethod]
        public void ShouldReturnUpWhenInputLineIsInUpTrend()
        {
            var trendLine = new double[] { 100, 101, 102, 103, 104, 105 };
            var analyzer = new TrendAnalyzer();
            var trend = analyzer.Analyze(trendLine);
            Assert.AreEqual(Trend.Up, trend);
        }

        [TestMethod]
        public void ShouldReturnDownWhenInputLineIsInDownTrend()
        {
            var trendLine = new double[] { 100, 99, 98, 97, 96, 95 };
            var analyzer = new TrendAnalyzer();
            var trend = analyzer.Analyze(trendLine);
            Assert.AreEqual(Trend.Up, trend);
        }

        [TestMethod]
        public void ShouldReturnSidewaysWhenInputLineIsInSidewayTrend()
        {
            var trendLine = new double[] { 100, 100, 100, 100, 100, 100 };
            var analyzer = new TrendAnalyzer();
            var trend = analyzer.Analyze(trendLine);
            Assert.AreEqual(Trend.Sideways, trend);
        }
    }
}
