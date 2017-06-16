using System;
using Rabun.Oanda.Rest.Endpoints;
using TradeAppSample.Common;
using System.Threading.Tasks;
using System.Linq;
using Rabun.Oanda.Rest.Models;

namespace TradeAppSample.Setup
{
    class SetupService
    {
        private InstrumentModel instrument;
        private int accountId;
        private AccountEndpoints accountEndpoints;
        private RateEndpoints rateEndpoints;

        public SetupService(InstrumentModel instrument, int accountId, AccountEndpoints accountEndpoints, RateEndpoints rateEndpoints)
        {
            this.instrument = instrument;
            this.accountId = accountId;
            this.accountEndpoints = accountEndpoints;
            this.rateEndpoints = rateEndpoints;
        }

        public async Task<SetupResult> Setup(TradeType tradeType)
        {
            var result = new SetupResult();

            var account = await accountEndpoints.GetAccountDetails(accountId);

            var currentRate = (await rateEndpoints.GetPrices(instrument.Instrument)).First();
            var currentPrice = (decimal)(tradeType == TradeType.Long ? currentRate.Ask : currentRate.Bid);

            // 最近最大1日間の変動幅を取得
            var candle = await rateEndpoints.GetCandles(instrument.Instrument, OandaTypes.GranularityType.H12, DateTime.Now.AddDays(-1.0), DateTime.Now);

            // 取引レンジの上限を取得
            var rangeMax = (decimal)(tradeType == TradeType.Long ? candle.Candles.Max(r => r.HighAsk) : candle.Candles.Max(r => r.HighBid));
            var rangeMin = (decimal)(tradeType == TradeType.Long ? candle.Candles.Min(r => r.LowAsk) : candle.Candles.Min(r => r.LowBid));

            var range = Math.Abs(rangeMax - rangeMin);

            // 差が0.1未満の場合は差を0.1として取引する
            if (range < 0.1m)
                range = 0.1m;

            // 目標値を設定
            result.GoalPrice = alignToPrecision(currentPrice + (tradeType == TradeType.Long ? range * 0.4m : range * -0.4m), (decimal)instrument.Precision);

            // ロスカットを設定
            result.StopLoss = alignToPrecision(currentPrice + (tradeType == TradeType.Long ? range * -0.2m : range * 0.2m), (decimal)instrument.Precision);

            // 最大の負け金額を計算(100回負けても大丈夫なように...)
            var lossPerTrade = (decimal)account.Balance / 100m;
            var maximumLossForThisTrade = Math.Abs(result.StopLoss - currentPrice);

            // ロスカットが１回の最大負けになるように取引数量を決定
            result.Units = (int)(lossPerTrade / maximumLossForThisTrade);

            // レバレッジを10バイトして計算
            var maximumUnits = (int)((decimal)account.MarginAvail / (currentPrice  * 1.1m * (decimal)instrument.MarginRate));
            if (result.Units > maximumUnits)
            {
                result.Units = maximumUnits;
            }

            // 保持期限を設定
            result.Expires = DateTime.Now.AddHours(4);

            return result;
        }

        private decimal alignToPrecision(decimal value, decimal precision)
        {
            return Math.Round(value / precision, 0) * precision;
        }
    }
}