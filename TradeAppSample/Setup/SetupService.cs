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
        private string instrument;
        private int accountId;
        private AccountEndpoints accountEndpoints;
        private RateEndpoints rateEndpoints;

        public SetupService(string instrument, int accountId, AccountEndpoints accountEndpoints, RateEndpoints rateEndpoints)
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

            var currentRate = (await rateEndpoints.GetPrices(instrument)).First();
            var currentPrice = (decimal)(tradeType == TradeType.Long ? currentRate.Ask : currentRate.Bid);

            // 直近一日の変動幅を取得
            var rate = (await rateEndpoints.GetCandles(instrument, OandaTypes.GranularityType.D, 1)).Candles.First();

            // 取引レンジの上限を取得
            var rangeMax = (decimal)(tradeType == TradeType.Long ? rate.HighAsk : rate.HighBid);
            var rangeMin = (decimal)(tradeType == TradeType.Long ? rate.LowAsk : rate.LowBid);

            var range = Math.Abs(rangeMax - rangeMin);

            // 差が0.1未満の場合は差を0.1として取引する
            if (range < 0.1m)
                range = 0.1m;

            // 目標値を設定
            result.GoalPrice = currentPrice + (tradeType == TradeType.Long ? range * 0.8m : range * -0.8m);

            // ロスカットを設定
            result.StopLoss = currentPrice + (tradeType == TradeType.Long ? range * -0.1m : range * 0.1m);

            // 最大の負け金額を計算(100回負けても大丈夫なように...)
            var lossPerTrade = (decimal)account.Balance / 100m;
            var maximumLossForThisTrade = Math.Abs(result.StopLoss - currentPrice);

            // ロスカットが１回の最大負けになるように取引数量を決定
            result.Units = (int)(lossPerTrade / maximumLossForThisTrade);

            // 保持期限を設定
            result.Expires = DateTime.Now.AddHours(3);

            return result;
        }
    }
}