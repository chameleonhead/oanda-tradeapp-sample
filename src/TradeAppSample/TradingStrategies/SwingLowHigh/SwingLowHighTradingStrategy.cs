using TradeAppSample.ForexTrading;
using TradeAppSample.MarketData;

namespace TradeAppSample.TradingStrategies.SwingLowHigh
{
    public class SwingLowHighTradingStrategy : ITradingStrategy
    {
        private SwingLowHighContext context;
        private SwingLowHighSignalDetector detector = new SwingLowHighSignalDetector();
        private SwingLowHighContextFactory contextFactory = new SwingLowHighContextFactory();
        private ICandleProvider provider;

        public SwingLowHighTradingStrategy(ICandleProvider provider)
        {
            this.provider = provider;
        }

        public async void OnMarketContextUpdated(ForexMarketContext marketContext)
        {
            var candles = await provider.RequestCandleAsync(CandleRequest.Latest1H());
            context = contextFactory.Create(candles);
        }

        public void OnTick(Tick tick)
        {
            detector.DetectSignal(context, tick);
        }
    }
}
