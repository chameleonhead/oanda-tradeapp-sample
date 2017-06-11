using System;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Endpoints;
using TradeAppSample.Decision;
using TradeAppSample.Setup;
using Rabun.Oanda.Rest.Models;
using TradeAppSample.Common;
using System.Linq;
using System.Threading;

namespace TradeAppSample.Trade
{
    class Trader
    {
        private string instrument;
        private SetupService setupService;
        private RateEndpoints rateEndPoints;
        private TradeEndpoints tradeEndpoints;
        private OrderEndpoints orderEndPoints;

        public Trader(string instrument, SetupService setupService, RateEndpoints rateEndPoints, OrderEndpoints orderEndPoints, TradeEndpoints tradeEndpoints)
        {
            this.instrument = instrument;
            this.setupService = setupService;
            this.rateEndPoints = rateEndPoints;
            this.orderEndPoints = orderEndPoints;
            this.tradeEndpoints = tradeEndpoints;
        }

        public async Task Trade(DecisionResult decision)
        {
            // セットアップ
            var setup = await setupService.Setup(decision.TradeType);
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::セットアップ 目標値:{setup.GoalPrice}、期限:{setup.Expires.ToString("yyyy/MM/dd HH:mm:ss")}、取引数量:{setup.Units}、ストップロス:{setup.StopLoss}");

            var currentRate = (await rateEndPoints.GetPrices(instrument)).First();
            var currentPrice = (decimal)(decision.TradeType == TradeType.Long ? currentRate.Bid : currentRate.Ask);
            if (!canMakeOrder(decision.Price, currentPrice))
            {
                // オーダーの変動が想定以上の場合はオーダーしない
                return;
            }

            var openOrder = await CreateOrder(decision, setup);
            var basePrice = decision.Price;
            var goalPrice = setup.GoalPrice;
            var expires = setup.Expires;
            while (true)
            {
                try
                {
                    currentRate = (await rateEndPoints.GetPrices(instrument)).First();
                    if (currentRate.Status == "halted")
                    {
                        Thread.Sleep(1 * 60 * 1000);
                        continue;
                    }

                    currentPrice = (decimal)(decision.TradeType == TradeType.Long ? currentRate.Bid : currentRate.Ask);
                    var trades = await tradeEndpoints.GetTrades(instrument);
                    if (trades.Any())
                    {
                        Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::取引終了(自動売買実行のため) 現在のレート: {currentPrice}");
                        return;
                    }

                    if (shouldExtendGoal(decision.TradeType, goalPrice, currentPrice))
                    {
                        setup = await setupService.Setup(decision.TradeType);
                        foreach (var trade in trades)
                        {
                            // 全注文に対してストップロスを設定
                            await tradeEndpoints.UpdateTrade(trade.Id, (float)setup.StopLoss, null, null);
                        }
                    }
                    else
                    {
                        var expired = (expires < DateTime.Now);
                        if (expired)
                        {
                            // 取引が期限切れになった場合はすべてクローズする
                            foreach (var trade in trades)
                            {
                                await tradeEndpoints.CloseTrade(trade.Id);
                            }
                            return;
                        }
                    }
                }
                catch (AggregateException aggex)
                {
                    foreach (var ex in aggex.Flatten().InnerExceptions)
                    {
                        Console.Error.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{ex.Message}");
                        Console.Error.WriteLine(ex.StackTrace);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{ex.Message}");
                    Console.Error.WriteLine(ex.StackTrace);
                }
            }


        }

        private DateTime getNextOpenTime()
        {
            var theDate = DateTime.UtcNow;
            var d = theDate.AddDays(DayOfWeek.Monday - theDate.DayOfWeek);
            return d <= theDate ? d.AddDays(7) : d;
        }

        private bool shouldExtendGoal(TradeType tradeType, decimal goalPrice, decimal currentPrice)
        {
            if (currentPrice == goalPrice)
            {
                return true;
            }

            if (tradeType == TradeType.Long)
            {
                // 買いの場合は目標価格より上がった場合
                return currentPrice > goalPrice;
            }
            else
            {
                // 売りの場合は目標価格より下がった場合
                return currentPrice < goalPrice;
            }
        }

        private bool canMakeOrder(decimal price, decimal currentPrice)
        {
            // 差が0.1%未満なら売買する
            var diff = Math.Abs(price - currentPrice);
            return diff == 0 || (price / diff) < 0.001m;
        }

        private async Task<OrderOpen> CreateOrder(DecisionResult decision, SetupResult setup)
        {
            OandaTypes.Side side = decision.TradeType == TradeType.Long ? OandaTypes.Side.buy : OandaTypes.Side.sell;
            var order = await orderEndPoints.CreateMarketOrder(instrument, setup.Units, side);
            Console.WriteLine("{0:yyyy/MM/dd HH:mm:ss}::注文を実行: {1} にて約定", order.Time, order.Price);
            var trades = await tradeEndpoints.GetTrades(instrument);
            var profit = setup.GoalPrice - (decimal)order.Price;
            foreach (var trade in trades)
            {
                // 全注文に対してストップロスを設定
                await tradeEndpoints.UpdateTrade(trade.Id, (float)setup.StopLoss, null, null);
            }
            return order;
        }
    }
}