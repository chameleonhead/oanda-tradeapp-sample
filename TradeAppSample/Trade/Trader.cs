using System;
using System.Threading.Tasks;
using TradeAppSample.Decision;
using TradeAppSample.Setup;

namespace TradeAppSample.Trade
{
    class Trader
    {
        private DecisionResult decision;
        private SetupResult setup;

        public Trader(DecisionResult decision, SetupResult setup)
        {
            this.decision = decision;
            this.setup = setup;
        }

        public async Task Trade()
        {
        }
    }
}