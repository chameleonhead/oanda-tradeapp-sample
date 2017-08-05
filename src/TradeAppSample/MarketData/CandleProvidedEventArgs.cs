using System;
using TradeAppSample.ForexTrading;

namespace TradeAppSample.MarketData
{
    public class CandleProvidedEventArgs : EventArgs
    {
        public CandleProvidedEventArgs(Guid requestId, CandleRequest request, ForexCandle[] requestedCandles)
        {
            RequestId = requestId;
            Request = request;
            Candles = requestedCandles;
        }

        public Guid RequestId { get; }
        public CandleRequest Request { get; }
        public ForexCandle[] Candles { get; }
    }
}