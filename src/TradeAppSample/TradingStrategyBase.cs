using System.Threading.Tasks;

namespace TradeAppSample
{
    public abstract class TradingStrategyBase
    {
        public abstract void UpdateState(MarketContext marketContext);
        public abstract void OnTick(Tick tick);
    }
}