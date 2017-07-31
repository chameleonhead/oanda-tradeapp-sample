namespace TradeAppSample.ForexTrading.TradingStrategies.SwingLowHigh
{
    public class SwingLowHighTradingStrategy : ITradingStrategy
    {
        private SwingLowHighContext context;
        private SwingLowHighSignalDetector detector = new SwingLowHighSignalDetector();
        private SwingLowHighContextFactory contextFactory = new SwingLowHighContextFactory();

        public void OnMarketContextUpdated(MarketContext marketContext)
        {
            var candles = marketContext.ShortTermCandles;
            context = contextFactory.Create(candles);
        }

        public void OnTick(Tick tick)
        {
            detector.DetectSignal(context, tick);
        }
    }
}
