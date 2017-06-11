﻿using System;
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

namespace TradeAppSample
{
    public class TradeApp
    {
        public TradeApp(string key, int accountId)
        {
            var factory = new DefaultFactory(key, AccountType.practice, accountId);
            OrderEndPoints = factory.GetEndpoint<OrderEndpoints>();
            PositionEndpoints = factory.GetEndpoint<PositionEndpoints>();
            RateEndpoints = factory.GetEndpoint<RateEndpoints>();
            TradeEndpoints = factory.GetEndpoint<TradeEndpoints>();
            TransactionEndpoints = factory.GetEndpoint<TransactionEndpoints>();
        }

        public OrderEndpoints OrderEndPoints { get; internal set; }
        public PositionEndpoints PositionEndpoints { get; internal set; }
        public RateEndpoints RateEndpoints { get; internal set; }
        public TradeEndpoints TradeEndpoints { get; internal set; }
        public TransactionEndpoints TransactionEndpoints { get; internal set; }

        /// <summary>
        /// プログラム実行ループ
        /// </summary>
        public void Run()
        {
            var instrument = "USD_JPY";
            var decisionService = new DecisionService(instrument, RateEndpoints);
            var setupService = new SetupService(instrument, RateEndpoints);

            while (!CancelRequested())
            {
                try
                {
                    // 売買するか決定
                    var decision = decisionService.Decide().Result;
                    if (decision.ShouldTrade)
                    {
                        // セットアップ
                        var setup = setupService.Setup(decision.TradeType).Result;

                        // トレード開始
                        var trader = new Trader(decision, setup);
                        trader.Trade().Wait();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                }

                // 1分待って次のトレード
                Thread.Sleep(1 * 60 * 1000);
            }
        }

        private bool CancelRequested()
        {
            // TODO ファイルを監視したりしてキャンセルできるようにする
            return true;
        }
    }
}
