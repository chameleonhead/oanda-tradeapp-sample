namespace TradeAppSample
{
    public class SwingLowHighTradingStrategy : TradingStrategyBase
    {
        private SwingLowHighContext context;
        private SwingLowHighSignalDetector detector = new SwingLowHighSignalDetector();
        private SwingLowHighContextFactory contextFactory = new SwingLowHighContextFactory();

        public override void UpdateState(MarketContext marketContext)
        {
            var candles = marketContext.ShortTermCandles;
            context = contextFactory.Create(candles);
        }

        public override void OnTick(Tick tick)
        {
            detector.DetectSignal(context, tick);
        }
    }
}
