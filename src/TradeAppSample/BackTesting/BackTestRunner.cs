using System;
using System.Threading.Tasks;
using TradeAppSample.MarketData;

namespace TradeAppSample.BackTesting
{
    public class BackTestRunner
    {
        private ICandleProvider candleProvider;
        private ITickProvider tickProvider;

        public BackTestRunner(ICandleProvider candleProvider, ITickProvider tickProvider)
        {
            this.candleProvider = candleProvider;
            this.tickProvider = tickProvider;
        }

        public async Task Run()
        {

        }
    }
}
