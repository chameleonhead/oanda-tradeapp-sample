using TradeAppSample.Common;

namespace TradeAppSample.Decision
{
    public class DecisionResult
    {
        public bool ShouldTrade { get; internal set; }
        public TradeType TradeType { get; internal set; }
    }
}