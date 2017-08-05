using System;
using System.Threading.Tasks;
using TradeAppSample.ForexTrading;

namespace TradeAppSample.MarketData
{
    public interface ICandleProvider
    {
        Task<ForexCandle[]> RequestCandleAsync(CandleRequest request);
        void RequestCandle(CandleRequest request);
        event EventHandler<CandleProvidedEventArgs> CandleProvided;
    }
}
