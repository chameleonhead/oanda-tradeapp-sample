using System;
using Rabun.Oanda.Rest.Endpoints;
using TradeAppSample.Common;
using System.Threading.Tasks;

namespace TradeAppSample.Setup
{
    class SetupService
    {
        private string instrument;
        private RateEndpoints rateEndpoints;

        public SetupService(string instrument, RateEndpoints rateEndpoints)
        {
            this.instrument = instrument;
            this.rateEndpoints = rateEndpoints;
        }

        public async Task<SetupResult> Setup(TradeType tradeType)
        {
            var result = new SetupResult();
            return result;
        }
    }
}