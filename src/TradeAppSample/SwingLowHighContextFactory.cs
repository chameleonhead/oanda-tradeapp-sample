using System.Linq;

namespace TradeAppSample
{
    public class SwingLowHighContextFactory
    {
        public SwingLowHighContext Create(ForexCandle[] candles)
        {
            var high = candles.Max(x => x.High);
            var low = candles.Min(x => x.Low);
            return new SwingLowHighContext(high, low);
        }
    }
}
