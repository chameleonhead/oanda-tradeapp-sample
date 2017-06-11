using System;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Endpoints;
using TradeAppSample.Decision;
using TradeAppSample.Setup;

namespace TradeAppSample.Trade
{
    class Trader
    {
        private string instrument;
        private TradeEndpoints tradeEndpoints;
        private OrderEndpoints orderEndPoints;

        public Trader(string instrument, OrderEndpoints orderEndPoints, TradeEndpoints tradeEndpoints)
        {
            this.instrument = instrument;
            this.orderEndPoints = orderEndPoints;
            this.tradeEndpoints = tradeEndpoints;
        }

        public async Task Trade(DecisionResult decision, SetupResult setup)
        {
        }
    }
}