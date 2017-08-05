namespace TradeAppSample.MarketData
{
    public class CandleRequest
    {
        public CandleRequest(CandleGranularity granularity)
        {

        }
        public CandleGranularity Granularity { get; }

        public static CandleRequest Latest1H()
        {
            return new CandleRequest(CandleGranularity.H1);
        }
    }
}