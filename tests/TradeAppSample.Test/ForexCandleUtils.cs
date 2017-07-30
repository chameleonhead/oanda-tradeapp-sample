using System;

namespace TradeAppSample.Test
{
    static class ForexCandleUtils
    {
        public static ForexCandle Create(decimal openBid, decimal highBid, decimal lowBid, decimal closeBid, decimal spread = 1)
        {
            var open = new ForexPrice(openBid + spread, openBid);
            var high = new ForexPrice(highBid + spread, highBid);
            var low = new ForexPrice(lowBid + spread, lowBid);
            var close = new ForexPrice(closeBid + spread, closeBid);
            return new ForexCandle(open, high, low, close);
        }

        public static ForexCandle CreateBig(decimal openBid, decimal closeBid, decimal spread = 1)
        {
            var highBid = (openBid + closeBid) / 2m * 1.3m;
            var lowBid = (openBid + closeBid) / 2m * 0.7m;
            return Create(openBid, highBid, lowBid, closeBid, spread);
        }

        public static ForexCandle CreateSmall(decimal openBid, decimal closeBid, decimal spread = 1)
        {
            var highBid = (openBid + closeBid) / 2m * 1.1m;
            var lowBid = (openBid + closeBid) / 2m * 0.9m;
            return Create(openBid, highBid, lowBid, closeBid, spread);
        }
    }
}