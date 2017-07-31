namespace TradeAppSample.ForexTrading
{
    public class MarketContext
    {
        public MarketContext(ForexCandle[] shortTermCandles, ForexCandle[] midTermCandles, ForexCandle[] longTermCandles)
        {
            ShortTermCandles = shortTermCandles;
            MidTermCandles = midTermCandles;
            LongTermCandles = longTermCandles;
        }

        public ForexCandle[] ShortTermCandles { get; internal set; }
        public ForexCandle[] MidTermCandles { get; internal set; }
        public ForexCandle[] LongTermCandles { get; internal set; }
    }
}