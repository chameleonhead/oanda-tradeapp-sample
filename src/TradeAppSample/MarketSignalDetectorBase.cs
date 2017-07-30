using System;

namespace TradeAppSample
{
    public abstract class MarketSignalDetectorBase
    {
        protected virtual void OnBuySignalDetected(BuySignal signal)
        {
            EventBus.Publish(new BuySignalDetectedEvent(DateTime.UtcNow, signal));
        }

        protected virtual void OnSellSignalDetected(SellSignal signal)
        {
            EventBus.Publish(new SellSignalDetectedEvent(DateTime.UtcNow, signal));
        }
    }
}
