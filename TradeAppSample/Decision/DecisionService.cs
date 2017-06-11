using Rabun.Oanda.Rest.Endpoints;
using System.Threading.Tasks;

namespace TradeAppSample.Decision
{
    class DecisionService
    {
        private string instrument;
        private RateEndpoints rateEndpoints;

        public DecisionService(string instrument, RateEndpoints rateEndpoints)
        {
            this.instrument = instrument;
            this.rateEndpoints = rateEndpoints;
        }

        public async Task<DecisionResult> Decide()
        {
            var result = new DecisionResult();
            return result;
        }
    }
}