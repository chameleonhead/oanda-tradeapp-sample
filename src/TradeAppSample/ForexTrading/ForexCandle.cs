namespace TradeAppSample.ForexTrading
{
    public class ForexCandle
    {
        public ForexCandle(decimal openAsk, decimal openBid, decimal highAsk, decimal highBid, decimal lowAsk, decimal lowBid, decimal closeAsk, decimal closeBid)
        {
            Open = new ForexPrice(openAsk, openBid);
            High = new ForexPrice(highAsk, highBid);
            Low = new ForexPrice(lowAsk, lowBid);
            Close = new ForexPrice(closeAsk, closeBid);
        }

        public ForexCandle(ForexPrice open, ForexPrice high, ForexPrice low, ForexPrice close)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        public ForexPrice Open { get; set; }
        public ForexPrice High { get; set; }
        public ForexPrice Low { get; set; }
        public ForexPrice Close { get; set; }
    }
}
