using TradeAppSample.Common;

namespace TradeAppSample.Decision
{
    public class DecisionResult
    {
        public bool ShouldTrade { get; internal set; }
        public bool Halted { get; internal set; }
        public CurrencyRate Rate { get; internal set; }
        public TradeType TradeType { get; internal set; }
        public decimal Price { get; internal set; }
    }
}