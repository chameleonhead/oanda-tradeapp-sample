using System;
using Rabun.Oanda.Rest.Endpoints;
using TradeAppSample.Common;
using System.Threading.Tasks;
using System.Linq;

namespace TradeAppSample.Setup
{
    class SetupService
    {
        private string instrument;
        private AccountEndpoints accountEndPoints;
        private RateEndpoints rateEndpoints;

        public SetupService(string instrument, AccountEndpoints accountEndPoints, RateEndpoints rateEndpoints)
        {
            this.instrument = instrument;
            this.accountEndPoints = accountEndPoints;
            this.rateEndpoints = rateEndpoints;
        }

        public async Task<SetupResult> Setup(TradeType tradeType)
        {
            var result = new SetupResult();
            return result;
        }
    }
}