using System;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Endpoints;
using TradeAppSample.Decision;
using TradeAppSample.Setup;
using Rabun.Oanda.Rest.Models;
using TradeAppSample.Common;
using System.Linq;

namespace TradeAppSample.Trade
{
    class Trader
    {
        private InstrumentModel instrument;
        private SetupService setupService;
        private RateEndpoints rateEndPoints;
        private TradeEndpoints tradeEndpoints;
        private OrderEndpoints orderEndPoints;

        public Trader(InstrumentModel instrument, SetupService setupService, RateEndpoints rateEndPoints, OrderEndpoints orderEndPoints, TradeEndpoints tradeEndpoints)
        {
            this.instrument = instrument;
            this.setupService = setupService;
            this.rateEndPoints = rateEndPoints;
            this.orderEndPoints = orderEndPoints;
            this.tradeEndpoints = tradeEndpoints;
        }

        public async Task<TradeResult> Trade(DecisionResult decision)
        {
            // セットアップ
            var setup = await setupService.Setup(decision.TradeType);
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{instrument.DisplayName} セットアップ 目標値:{setup.GoalPrice}、期限:{setup.Expires.ToString("yyyy/MM/dd HH:mm:ss")}、取引数量:{setup.Units}、ストップロス:{setup.StopLoss}");

            // 激しい相場ではないことを確認するため、1秒待ってから処理継続
            await Task.Delay(1000);

            var currentRate = (await rateEndPoints.GetPrices(instrument.Instrument)).First();
            var currentPrice = (decimal)(decision.TradeType == TradeType.Long ? currentRate.Bid : currentRate.Ask);
            if (!canMakeOrder(decision.Price, currentPrice))
            {
                // オーダーの変動が想定以上の場合はオーダーしない
                return null;
            }

            var openOrder = await CreateOrder(decision, setup);
            var tradeResult = new TradeResult();
            tradeResult.Instrument = instrument.Instrument;
            tradeResult.InstrumentName = instrument.DisplayName;
            tradeResult.TradeType = decision.TradeType;
            tradeResult.DecisionPrice = decision.Price;
            tradeResult.StartedPrice = (decimal)openOrder.Price;
            tradeResult.DealStartedAt = openOrder.Time;
            tradeResult.FirstGoalPrice = setup.GoalPrice;
            tradeResult.FirstStopLoss = setup.StopLoss;

            var basePrice = decision.Price;
            var goalPrice = setup.GoalPrice;
            var stoplossPrice = setup.StopLoss;
            var expires = setup.Expires;
            while (true)
            {
                try
                {
                    currentRate = (await rateEndPoints.GetPrices(instrument.Instrument)).First();
                    if (currentRate.Status == "halted")
                    {
                        await Task.Delay(1 * 60 * 1000);
                        continue;
                    }

                    currentPrice = (decimal)(decision.TradeType == TradeType.Long ? currentRate.Bid : currentRate.Ask);
                    var trades = await tradeEndpoints.GetTrades(instrument.Instrument);
                    if (!trades.Any())
                    {
                        tradeResult.FinishedPrice = currentPrice;
                        tradeResult.DealFinishedAt = DateTime.Now;
                        Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{instrument.DisplayName} 取引終了(自動売買実行のため) 現在のレート: {currentPrice}");
                        return tradeResult;
                    }

                    if (shouldChangeGoal(decision.TradeType, basePrice, goalPrice, currentPrice))
                    {
                        var nextGoal = calculateNextGoal(decision.TradeType, basePrice, goalPrice, currentPrice);
                        basePrice = currentPrice;
                        stoplossPrice = nextGoal.StopLoss;
                        goalPrice = nextGoal.GoalPrice;
                        expires = nextGoal.Expires;
                        tradeResult.AddGoal(goalPrice, basePrice, expires);
                        foreach (var trade in trades)
                        {
                            // 全注文に対してストップロスを設定
                            await tradeEndpoints.UpdateTrade(trade.Id, (float)basePrice, null, null);
                        }
                        Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{instrument.DisplayName} 目標値更新 現在価格: {currentPrice}、新しい目標値:{goalPrice}、期限:{expires.ToString("yyyy/MM/dd HH:mm:ss")}、ストップロス:{basePrice}");
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
                            tradeResult.FinishedPrice = currentPrice;
                            tradeResult.DealFinishedAt = DateTime.Now;
                            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{instrument.DisplayName} 取引終了(期限までに目標値を未達成のため) 現在のレート: {currentPrice}");
                            return tradeResult;
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

        private NextGoal calculateNextGoal(TradeType tradeType, decimal basePrice, decimal goalPrice, decimal currentPrice)
        {
            var diff = Math.Abs(goalPrice - basePrice);
            if (tradeType == TradeType.Long)
            {
                // 買いの場合は目標利益の50%より上がった場合
                return new NextGoal()
                {
                    GoalPrice = currentPrice + diff,
                    StopLoss = currentPrice - round(diff * 0.5m),
                    Expires = DateTime.Now.AddHours(4)
                };
            }
            else
            {
                // 売りの場合は目標利益の50%より下がった場合
                return new NextGoal()
                {
                    GoalPrice = currentPrice - diff,
                    StopLoss = currentPrice + round(diff * 0.5m),
                    Expires = DateTime.Now.AddHours(4)
                };
            }
        }

        private decimal round(decimal value)
        {
            return Math.Round(value / (decimal)instrument.Precision, 0) * (decimal)instrument.Precision;
        }

        private bool shouldChangeGoal(TradeType tradeType, decimal basePrice, decimal goalPrice, decimal currentPrice)
        {
            var diff = Math.Abs(goalPrice - basePrice);
            if (tradeType == TradeType.Long)
            {
                // 買いの場合は目標利益の50%より上がった場合
                return currentPrice > (basePrice + diff * 0.5m);
            }
            else
            {
                // 売りの場合は目標利益の50%より下がった場合
                return currentPrice < (basePrice - diff * 0.5m);
            }
        }

        private bool canMakeOrder(decimal price, decimal currentPrice)
        {
            // 差が20pips未満なら売買する
            var diff = Math.Abs(price - currentPrice);
            return diff == 0 || (diff / price) < ((decimal)instrument.Pip * 20m);
        }

        private async Task<OrderOpen> CreateOrder(DecisionResult decision, SetupResult setup)
        {
            OandaTypes.Side side = decision.TradeType == TradeType.Long ? OandaTypes.Side.buy : OandaTypes.Side.sell;
            var order = await orderEndPoints.CreateMarketOrder(instrument.Instrument, setup.Units, side);
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{instrument.DisplayName} 注文を実行: {order.Price} にて約定 ({order.Time.ToString("yyyy/MM/dd HH:mm:ss")})");
            var trades = await tradeEndpoints.GetTrades(instrument.Instrument);
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