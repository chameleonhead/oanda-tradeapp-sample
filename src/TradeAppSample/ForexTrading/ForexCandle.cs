namespace TradeAppSample.ForexTrading
{
    public class ForexCandle
    {
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
