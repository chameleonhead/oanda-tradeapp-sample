using System;
using TradeAppSample.ForexTrading;
using TradeAppSample.Shared;

namespace TradeAppSample.TradingStrategies.MarketSignalDetection
{
    public abstract class MarketSignalDetectorBase
    {
        protected virtual void OnBuySignalDetected(Tick currentTick)
        {
            EventBus.Publish(new BuySignalDetectedEvent(DateTime.UtcNow, new BuySignal(currentTick)));
        }

        protected virtual void OnSellSignalDetected(Tick currentTick)
        {
            EventBus.Publish(new SellSignalDetectedEvent(DateTime.UtcNow, new SellSignal(currentTick)));
        }
    }
}
