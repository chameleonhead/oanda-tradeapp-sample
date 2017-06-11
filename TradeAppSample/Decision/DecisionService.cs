﻿using Rabun.Oanda.Rest.Endpoints;
using Rabun.Oanda.Rest.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TradeAppSample.Common;

namespace TradeAppSample.Decision
{
    class DecisionService
    {
        private string instrument;
        private RateEndpoints rateEndpoints;

        public DecisionService(string instrument, RateEndpoints rateEndpoints)
        {
            this.instrument = instrument;
            this.rateEndpoints = rateEndpoints;
        }

        public async Task<DecisionResult> Decide()
        {
            var result = new DecisionResult();

            var currentRate = (await rateEndpoints.GetPrices(instrument)).First();
            if (currentRate.Status == "HALTED")
            {
                result.Halted = true;
                return result;
            }

            // 売買するかどうかを決定
            var rand = new Random(DateTime.Now.Millisecond);
            result.ShouldTrade = (rand.Next() % 10 == 0);
            if (!result.ShouldTrade)
                return result;

            // 売買するなら買い・売りを決定
            var rates = await rateEndpoints.GetCandles(instrument, OandaTypes.GranularityType.D, 10);

            // 5日間移動平均線の取得
            var emaLine5d = calculateEmaLine(rates.Candles.Select(c => c.CloseAsk).Cast<decimal>().ToArray(), 5);

            // 移動平均が上がり(正なら買い、0、負なら売り)
            result.TradeType = emaLine5d.Average() > 0 ? TradeType.Long : TradeType.Short;

            return result;
        }

        /// <summary>
        /// 指数平滑移動平均線の計算
        /// </summary>
        /// <param name="points"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private decimal[] calculateEmaLine(decimal[] points, int count)
        {
            int length = points.Length - count;
            var line = new List<decimal>();
            for (int i = 0; i < length; i++)
            {
                line.Add(calculateEma(points.Skip(i).Take(count).ToArray()));
            }
            return line.ToArray();
        }

        /// <summary>
        /// 指数平滑移動平均の計算
        /// 計算式： (1日目価格 + 2日目価格 + 3日目価格 + 3日目価格) / 4
        /// </summary>
        /// <param name="enumerable"></param>
        private decimal calculateEma(decimal[] points)
        {
            var prices = points.Sum() + points.Last();
            return prices / (points.Length + 1);
        }
    }
}