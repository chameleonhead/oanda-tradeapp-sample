using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Endpoints;
using Rabun.Oanda.Rest.Factories;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;
using TradeAppSample.Decision;
using TradeAppSample.Setup;
using TradeAppSample.Trade;
using System.Threading;
using System.Xml.Serialization;
using System.IO;

namespace TradeAppSample
{
    public class TradeApp
    {
        private int accountId;
        public TradeApp(string key, int accountId)
        {
            var factory = new DefaultFactory(key, AccountType.practice, accountId);
            OrderEndPoints = factory.GetEndpoint<OrderEndpoints>();
            PositionEndpoints = factory.GetEndpoint<PositionEndpoints>();
            RateEndpoints = factory.GetEndpoint<RateEndpoints>();
            TradeEndpoints = factory.GetEndpoint<TradeEndpoints>();
            TransactionEndpoints = factory.GetEndpoint<TransactionEndpoints>();
            AccountEndpoints = new AccountEndpoints(key, AccountType.practice);

            this.accountId = accountId;
        }

        public AccountEndpoints AccountEndpoints { get; private set; }
        public OrderEndpoints OrderEndPoints { get; private set; }
        public PositionEndpoints PositionEndpoints { get; private set; }
        public RateEndpoints RateEndpoints { get; private set; }
        public TradeEndpoints TradeEndpoints { get; private set; }
        public TransactionEndpoints TransactionEndpoints { get; private set; }

        /// <summary>
        /// プログラム実行ループ
        /// </summary>
        public void Run()
        {
            if (File.Exists("shutdown"))
            {
                File.Delete("shutdown");
            }

            var instruments = RateEndpoints.GetInstruments("instrument,displayName,pip,maxTradeUnits,precision,maxTrailingStop,minTrailingStop,marginRate,halted", "AUD_JPY,CAD_JPY,CHF_JPY,EUR_JPY,GBP_JPY,HKD_JPY,NZD_JPY,SGD_JPY,TRY_JPY,USD_JPY,ZAR_JPY").Result;
            while (!cancelRequested())
            {
                try
                {
                    // 取引する通貨を決定
                    var rand = new Random(DateTime.Now.Millisecond);
                    var instrument = instruments[rand.Next() % instruments.Count];
                    // 売買するか決定
                    var decisionService = new DecisionService(instrument, RateEndpoints);
                    var decision = decisionService.Decide().Result;
                    if (decision.ShouldTrade)
                    {
                        var setupService = new SetupService(instrument, accountId, AccountEndpoints, RateEndpoints);
                        var trader = new Trader(instrument, setupService, RateEndpoints, OrderEndPoints, TradeEndpoints);

                        // トレード開始
                        var tradeResult = trader.Trade(decision).Result;
                        printTradeResult(tradeResult);
                        saveTrade(tradeResult);
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

                // 1分待って次のトレード
                Thread.Sleep(1 * 60 * 1000);
            }
        }

        private void printTradeResult(TradeResult tradeResult)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}::{tradeResult.InstrumentName}");
            Console.WriteLine($@"
    価格:{tradeResult.StartedPrice} -> {tradeResult.FinishedPrice} (取引決定時: {tradeResult.DecisionPrice})
        ({tradeResult.DealStartedAt.ToString("MM/dd HH:mm:ss")} -> {tradeResult.DealFinishedAt.ToString("MM/dd HH:mm:ss")})

    [初回の状態]
        目標値    :{tradeResult.FirstGoalPrice}
        ロスカット:{tradeResult.FirstStopLoss}
");
        }

        private void saveTrade(TradeResult tradeResult)
        {
            using (var stream = new FileStream("Trade_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml", FileMode.Create, FileAccess.Write))
            {
                var serializer = new XmlSerializer(typeof(TradeResult));
                serializer.Serialize(stream, tradeResult);
            }
        }

        private bool cancelRequested()
        {
            return File.Exists("shutdown");
        }
    }
}
