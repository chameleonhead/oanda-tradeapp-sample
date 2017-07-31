using System;
using TradeAppSample.Shared;

namespace TradeAppSample.ForexTrading.MarketSignalDetection
{
    public abstract class MarketSignalDetectorBase
    {
        protected virtual void OnBuySignalDetected()
        {
            EventBus.Publish(new BuySignalDetectedEvent(DateTime.UtcNow, new BuySignal()));
        }

        protected virtual void OnSellSignalDetected()
        {
            EventBus.Publish(new SellSignalDetectedEvent(DateTime.UtcNow, new SellSignal()));
        }
    }
}
