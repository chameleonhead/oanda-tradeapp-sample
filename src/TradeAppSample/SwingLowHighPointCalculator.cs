using System.Linq;

namespace TradeAppSample
{
    public class SwingLowHighPointResult
    {
        internal SwingLowHighPointResult(ForexPrice high, ForexPrice low)
        {
            Low = low;
            High = high;
        }

        public ForexPrice Low { get; }
        public ForexPrice High { get; }
    }

    public class SwingLowHighPointCalculator
    {
        public SwingLowHighPointResult Calculate(ForexCandle[] candles)
        {
            var high = candles.Max(x => x.High);
            var low = candles.Min(x => x.Low);
            return new SwingLowHighPointResult(high, low);
        }
    }
}
