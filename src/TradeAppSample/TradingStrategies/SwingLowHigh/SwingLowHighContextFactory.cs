﻿using System.Linq;
using TradeAppSample.ForexTrading;

namespace TradeAppSample.TradingStrategies.SwingLowHigh
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
