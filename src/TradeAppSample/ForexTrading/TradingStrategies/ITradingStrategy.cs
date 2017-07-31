using TradeAppSample.ForexTrading;

namespace TradeAppSample.ForexTrading.TradingStrategies
{
    public interface ITradingStrategy
    {
        void OnMarketContextUpdated(MarketContext marketContext);
        void OnTick(Tick tick);
    }
}