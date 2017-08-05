using System;
using System.Collections.Generic;
using TradeAppSample.MarketData;

namespace TradeAppSample.ForexTrading
{
    public class ForexMarket
    {
        private ForexMarketContext context;
        private ITickProvider tickProvider;
        private ICandleProvider candleProvider;

        private List<IMarketListener> listeners;

        public ForexMarket(Instruments instrument, ITickProvider tickProvider, ICandleProvider candleProvider) : this(new ForexMarketContext(instrument, MarketState.Unknown), tickProvider, candleProvider)
        {
        }

        public ForexMarket(ForexMarketContext context, ITickProvider tickProvider, ICandleProvider candleProvider)
        {
            this.context = context;
            this.tickProvider = tickProvider;
            this.candleProvider = candleProvider;
            this.listeners = new List<IMarketListener>();
        }

        public MarketState CurrentState
        {
            get { return context.State; }
        }

        public void Subscribe(IMarketListener listener)
        {
        }

        public void Unsubscribe(IMarketListener listener)
        {
        }

        public void SetCurrentTime(DateTime now)
        {
        }

        public void Open()
        {
        }

        public void Close()
        {
        }
    }
}
