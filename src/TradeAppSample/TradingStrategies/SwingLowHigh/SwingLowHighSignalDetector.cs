﻿using TradeAppSample.ForexTrading;
using TradeAppSample.TradingStrategies.MarketSignalDetection;

namespace TradeAppSample.TradingStrategies.SwingLowHigh
{
    public class SwingLowHighSignalDetector : MarketSignalDetectorBase
    {
        public void DetectSignal(SwingLowHighContext context, Tick currentTick)
        {
            if (currentTick.Price > context.SwingHigh)
                OnBuySignalDetected(currentTick);
        }
    }
}
