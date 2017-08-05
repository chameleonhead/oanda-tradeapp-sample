namespace TradeAppSample.ForexTrading
{
    public interface IMarketListener
    {
        void OnMarketContextUpdated(ForexMarketContext context);
        void OnTick(Tick tick);
    }
}