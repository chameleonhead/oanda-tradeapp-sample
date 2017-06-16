﻿using Rabun.Oanda.Rest.Endpoints;
using Rabun.Oanda.Rest.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TradeAppSample.Common;
using System.Xml;

namespace TradeAppSample.Decision
{
    class DecisionService
    {
        private InstrumentModel instrument;
        private RateEndpoints rateEndpoints;

        public DecisionService(InstrumentModel instrument, RateEndpoints rateEndpoints)
        {
            this.instrument = instrument;
            this.rateEndpoints = rateEndpoints;
        }

        public async Task<DecisionResult> Decide()
        {
            var result = new DecisionResult();

            // 売買するかどうかを決定
            var rand = new Random(DateTime.Now.Millisecond);
            result.ShouldTrade = (rand.Next() % 10 == 0);
            if (!result.ShouldTrade)
                // 売買しないなら終了
                return result;

            // 現在の価格を取得
            var currentRate = (await rateEndpoints.GetPrices(instrument.Instrument)).First();
            if (currentRate.Status == "halted")
            {
                result.ShouldTrade = false;
                result.Halted = true;
                return result;
            }
            result.Rate = new CurrencyRate(XmlConvert.ToDateTime(currentRate.Time, XmlDateTimeSerializationMode.Local), (decimal)currentRate.Ask, (decimal)currentRate.Bid);
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{instrument.DisplayName} 市場価格: ASK:{result.Rate.Ask} BID:{result.Rate.Bid} ({result.Rate.Time.ToString("yyyy/MM/dd HH:mm:ss")}現在)");

            // 買い・売りを決定
            var rates = await rateEndpoints.GetCandles(instrument.Instrument, OandaTypes.GranularityType.H1, 8);

            // 5日間移動平均線の取得
            var emaLine6h = calculateEmaLine(rates.Candles.Select(c => (decimal)c.CloseAsk).ToArray(), 6);

            // 移動平均が上がりなら買い、下がりなら売り
            result.TradeType = emaLine6h.First() < emaLine6h.Last() ? TradeType.Long : TradeType.Short;
            result.Price = result.TradeType == TradeType.Long ? result.Rate.Ask : result.Rate.Bid;

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