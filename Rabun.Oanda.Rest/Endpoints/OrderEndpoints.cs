using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;

namespace Rabun.Oanda.Rest.Endpoints
{
    /// <summary>
    /// Order endpoints
    /// </summary>
    public class OrderEndpoints : Endpoint
    {
        private readonly string _ordersRoute = "/v1/accounts/:accountId/orders";
        private readonly string _orderRoute = "/v1/accounts/:accountId/orders/:orderId";
        private readonly int _accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderEndpoints"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="accountId">The account identifier.</param>
        public OrderEndpoints(string key, AccountType accountType, int accountId)
            : base(key, accountType)
        {
            _accountId = accountId;
        }

        #region GetOrders

        private class OrdersWrapper
        {
            public List<Order> Orders { get; set; }
        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders()
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, null, _ordersRoute);
            return ordersWrapper.Orders;

        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <param name="instrument">Retrieve open orders for a specific instrument only</param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders(string instrument)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, properties, _ordersRoute);
            return ordersWrapper.Orders;

        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <param name="instrument">Retrieve open orders for a specific instrument only</param>
        /// <param name="count">Maximum number of open orders to return. Default: 50. Max value: 500</param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders(string instrument, int count)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("count", count);

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, properties, _ordersRoute);
            return ordersWrapper.Orders;

        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <param name="instrument">Retrieve open orders for a specific instrument only</param>
        /// <param name="count">Maximum number of open orders to return. Default: 50. Max value: 500</param>
        /// <param name="maxId">The server will return orders with id less than or equal to this, in descending order (for pagination)</param>
        /// <param name="ids">An URL encoded comma (%2C) separated list of orders to retrieve. 
        /// Maximum number of ids: 50. No other parameter may be specified with the ids paramete</param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders(string instrument, int? count, long? maxId, string ids)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            if (count != null) properties.Add("count", count);
            if (maxId != null) properties.Add("maxId", maxId);
            if (!string.IsNullOrWhiteSpace(ids)) properties.Add("ids", ids);

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, properties, _ordersRoute);
            return ordersWrapper.Orders;

        }

        #endregion

        #region GetOrder

        /// <summary>
        /// Get information for an order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public async Task<Order> GetOrder(long orderId)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("orderId", orderId.ToString());

            Order order = await Get<Order>(routeParams, null, _orderRoute);
            return order;
        }

        #endregion

        #region CreateOrder

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="instrument">Required Instrument to open the order on</param>
        /// <param name="units">Required The number of units to open order for</param>
        /// <param name="side">Required Direction of the order, either "buy" or "sell"</param>
        /// <param name="type">Required The type of the order "limit", "stop", "marketIfTouched’ or "market"</param>
        /// <param name="expiry">Required If order type is "limit", "stop", or "marketIfTouched". The order expiration time in UTC. The value specified must be in a valid datetime format</param>
        /// <param name="price">Required If order type is "limit", "stop", or "marketIfTouched". The price where the order is set to trigger at</param>
        /// <param name="lowerBound">Optional The minimum execution price</param>
        /// <param name="upperBound">Optional The maximum execution price</param>
        /// <param name="stopLoss">The stop loss price</param>
        /// <param name="takeProfit">Optional The take profit price</param>
        /// <param name="trailingStop">Optional The trailing stop distance in pips, up to one decimal place</param>
        /// <returns></returns>
        public async Task<OrderOpen> CreateOrder(string instrument, int units, OandaTypes.Side side,
            OandaTypes.OrderType type, DateTime? expiry, float? price, float? lowerBound, float? upperBound, float? stopLoss, float? takeProfit, float? trailingStop)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("instrument", instrument);
            properties.Add("units", units.ToString());
            properties.Add("side", side.ToString());
            properties.Add("type", type.ToString());
            if (expiry != null) properties.Add("expiry", expiry.Value.ToString("o"));
            if (price != null) properties.Add("price", price.ToString());
            if (lowerBound != null) properties.Add("lowerBound", lowerBound.ToString());
            if (upperBound != null) properties.Add("upperBound", upperBound.ToString());
            if (stopLoss != null) properties.Add("stopLoss", stopLoss.ToString());
            if (takeProfit != null) properties.Add("takeProfit", takeProfit.ToString());
            if (trailingStop != null) properties.Add("trailingStop", trailingStop.ToString());

            if (type == OandaTypes.OrderType.market)
            {
                OrderMarketOpen orderMarket = await Post<OrderMarketOpen>(routeParams, properties, _ordersRoute);
                return orderMarket;
            }
            else
            {
                OrderCustumOpen customOrder = await Post<OrderCustumOpen>(routeParams, properties, _ordersRoute);
                return customOrder;
            }
        }

        /// <summary>
        /// Create a new market order
        /// </summary>
        /// <param name="instrument">Required Instrument to open the order on</param>
        /// <param name="units">Required The number of units to open order for</param>
        /// <param name="side">Required Direction of the order, either "buy" or "sell"</param>
        /// <returns></returns>
        public async Task<OrderMarketOpen> CreateMarketOrder(string instrument, int units, OandaTypes.Side side)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("instrument", instrument);
            properties.Add("units", units.ToString());
            properties.Add("side", side.ToString());
            properties.Add("type", OandaTypes.OrderType.market.ToString());

            OrderMarketOpen orderMarketOpen = await Post<OrderMarketOpen>(routeParams, properties, _ordersRoute);

            return orderMarketOpen;
        }

        /// <summary>
        /// Create a new market order
        /// </summary>
        /// <param name="instrument">Required Instrument to open the order on</param>
        /// <param name="units">Required The number of units to open order for</param>
        /// <param name="side">Required Direction of the order, either "buy" or "sell"</param>
        /// <param name="stopLoss">The stop loss price</param>
        /// <param name="takeProfit">Optional The take profit price</param>
        /// <returns></returns>
        public async Task<OrderMarketOpen> CreateMarketOrder(string instrument, int units, OandaTypes.Side side, float stopLoss, float takeProfit)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("instrument", instrument);
            properties.Add("units", units.ToString());
            properties.Add("side", side.ToString());
            properties.Add("stopLoss", stopLoss.ToString());
            properties.Add("takeProfit", takeProfit.ToString());
            properties.Add("type", OandaTypes.OrderType.market.ToString());

            OrderMarketOpen orderMarketOpen = await Post<OrderMarketOpen>(routeParams, properties, _ordersRoute);

            return orderMarketOpen;
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="instrument">Required Instrument to open the order on</param>
        /// <param name="units">Required The number of units to open order for</param>
        /// <param name="side">Required Direction of the order, either "buy" or "sell"</param>
        /// <param name="expiry">Required If order type is "limit", "stop", or "marketIfTouched". The order expiration time in UTC. The value specified must be in a valid datetime format</param>
        /// <param name="price">Required If order type is "limit", "stop", or "marketIfTouched". The price where the order is set to trigger at</param>
        /// <returns></returns>
        public async Task<OrderMarketIfTouchedOpen> CreateMarketIfTouchedOrder(string instrument, int units, OandaTypes.Side side, DateTime expiry, float price)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("instrument", instrument);
            properties.Add("units", units.ToString());
            properties.Add("side", side.ToString());
            properties.Add("type", OandaTypes.OrderType.marketIfTouched.ToString());
            properties.Add("expiry", expiry.ToString("o"));
            properties.Add("price", price.ToString());

            OrderMarketIfTouchedOpen orderMarketIfTouchedOpen = await Post<OrderMarketIfTouchedOpen>(routeParams, properties, _ordersRoute);

            return orderMarketIfTouchedOpen;
        }

        #endregion

        #region UpdateOrder

        public async Task<OrderMarketIfTouched> UpdateOrder(long orderId, int? units, float? price, DateTime? expiry,
            float? lowerBound, float? upperBound, float? stopLoss, float? takeProfit, int? trailingStop)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("orderId", orderId.ToString());

            Dictionary<string, string> properties = new Dictionary<string, string>();
            if (units != null) properties.Add("units", units.ToString());
            if (expiry != null) properties.Add("expiry", expiry.Value.ToString("o"));
            if (price != null) properties.Add("price", price.ToString());
            if (lowerBound != null) properties.Add("lowerBound", lowerBound.ToString());
            if (upperBound != null) properties.Add("upperBound", upperBound.ToString());
            if (takeProfit != null) properties.Add("takeProfit", takeProfit.ToString());
            if (trailingStop != null) properties.Add("trailingStop", trailingStop.ToString());

            OrderMarketIfTouched orderMarketIfTouched = await Patch<OrderMarketIfTouched>(routeParams, properties, _orderRoute);

            return orderMarketIfTouched;
        }

        #endregion

        #region CloseOrder

        /// <summary>
        /// Close order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public async Task<OrderClosed> CloseOrder(long orderId)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("orderId", orderId.ToString());

            OrderClosed orderClosed = await Delete<OrderClosed>(routeParams, _orderRoute);

            return orderClosed;
        }

        #endregion

    }
}
