using System;

namespace TradeAppSample.Trade
{
    public class NextGoal
    {
        public decimal GoalPrice { get; set; }
        public decimal StopLoss { get; set; }
        public DateTime Expires { get; set; }
    }
}