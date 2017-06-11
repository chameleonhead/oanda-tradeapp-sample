using System;

namespace TradeAppSample.Setup
{
    class SetupResult
    {
        public int Units { get; internal set; }
        public decimal GoalPrice { get; internal set; }
        public decimal StopLoss { get; internal set; }
        public DateTime Expires { get; internal set; }
    }
}