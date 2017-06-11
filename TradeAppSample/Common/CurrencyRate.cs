using System;

namespace TradeAppSample.Common
{
    public class CurrencyRate
    {
        public CurrencyRate(DateTime time, decimal ask, decimal bid)
        {
            Time = time;
            Ask = ask;
            Bid = bid;
        }

        public DateTime Time { get; private set; }
        public decimal Ask { get; private set; }
        public decimal Bid { get; private set; }
    }
}