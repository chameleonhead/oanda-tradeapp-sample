using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Endpoints;
using Rabun.Oanda.Rest.Factories;
using Rabun.Oanda.Rest.Base;

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

        }
    }
}
