using TradeAppSample.ForexTrading.MarketSignalDetection;

namespace TradeAppSample.ForexTrading.TradingStrategies.SwingLowHigh
{
    public class SwingLowHighSignalDetector : MarketSignalDetectorBase
    {
        public void DetectSignal(SwingLowHighContext context, Tick currentTick)
        {
            if (currentTick.Price > context.SwingHigh)
                OnBuySignalDetected();
        }
    }
}
