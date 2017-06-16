using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeAppSample.Common;

namespace TradeAppSample.Trade
{
    public class TradeResult
    {
        public TradeResult()
        {
            NextGoals = new List<NextGoal>();
        }

        public string Instrument { get; set; }
        public string InstrumentName { get; set; }
        public TradeType TradeType { get; set; }
        public decimal DecisionPrice { get; set; }
        public decimal StartedPrice { get; set; }
        public decimal FinishedPrice { get; set; }
        public DateTime DealStartedAt { get; set; }
        public DateTime DealFinishedAt { get; set; }
        public decimal FirstGoalPrice { get; set; }
        public decimal FirstStopLoss { get; set; }
        public List<NextGoal> NextGoals { get; set; }

        public void AddGoal(decimal goalPrice, decimal stopLoss, DateTime expires)
        {
            NextGoals.Add(new NextGoal() { GoalPrice = goalPrice, StopLoss = stopLoss, Expires = expires });
        }
    }
}
