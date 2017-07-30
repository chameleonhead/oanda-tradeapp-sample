namespace TradeAppSample
{
    public class SwingLowHighContext
    {
        public SwingLowHighContext(ForexPrice swingHigh, ForexPrice swingLow)
        {
            SwingHigh = swingHigh;
            SwingLow = swingLow;
        }

        public ForexPrice SwingHigh { get; }
        public ForexPrice SwingLow { get; }
    }
}
