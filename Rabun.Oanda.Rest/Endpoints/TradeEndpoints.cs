using System.Collections.Generic;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;
using System.Linq;
using System;

namespace Rabun.Oanda.Rest.Endpoints
{
    /// <summary>
    /// Trade endpoints
    /// </summary>
    public class TradeEndpoints : Endpoint
    {
        private readonly string _tradesRoute = "/v1/accounts/:accountId/trades";
        private readonly string _tradeRoute = "/v1/accounts/:accountId/trades/:tradeId";
        private readonly int _accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeEndpoints"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="accountId">The account identifier.</param>
        public TradeEndpoints(string key, AccountType accountType, int accountId)
            : base(key, accountType)
        {
            _accountId = accountId;
        }


        #region GetTrades

        private class TradesWrapper
        {
            public List<Trade> Trades { get; set; }
        }

        /// <summary>
        /// Get a list of open trades
        /// </summary>
        /// <returns></returns>
        public async Task<List<Trade>> GetTrades()
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            TradesWrapper tradesWrapper = await Get<TradesWrapper>(routeParams, null, _tradesRoute);
            return tradesWrapper.Trades;
        }

        /// <summary>
        /// Get a list of open trades
        /// </summary>
        /// <param name="instrument">Retrieve open trades for a specific instrument only</param>
        /// <returns></returns>
        public async Task<List<Trade>> GetTrades(string instrument)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);

            TradesWrapper tradesWrapper = await Get<TradesWrapper>(routeParams, properties, _tradesRoute);
            return tradesWrapper.Trades;
        }

        /// <summary>
        /// Get a list of open trades
        /// </summary>
        /// <param name="instrument">Retrieve open trades for a specific instrument only</param>
        /// <param name="count">Maximum number of open trades to return. Default: 50 Max value: 500</param>
        /// <returns></returns>
        public async Task<List<Trade>> GetTrades(string instrument, int count)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("count", count);

            TradesWrapper tradesWrapper = await Get<TradesWrapper>(routeParams, properties, _tradesRoute);
            return tradesWrapper.Trades;
        }

        /// <summary>
        /// Get a list of open trades
        /// </summary>
        /// <param name="instrument">Retrieve open trades for a specific instrument only</param>
        /// <param name="count">Maximum number of open trades to return. Default: 50 Max value: 500</param>
        /// <param name="maxId">The server will return trades with id less than or equal to this, in descending order (for pagination)</param>
        /// <param name="ids"> A (URL encoded) comma separated list of trades to retrieve. Maximum number of ids: 50. No other parameter may be specified with the ids parameter</param>
        /// <returns></returns>
        public async Task<List<Trade>> GetTrades(string instrument, int? count, long? maxId, string ids)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            if (count != null) properties.Add("count", count);
            if (maxId != null) properties.Add("maxId", maxId);
            if (!string.IsNullOrWhiteSpace(ids)) properties.Add("ids", ids);

            TradesWrapper tradesWrapper = await Get<TradesWrapper>(routeParams, properties, _tradesRoute);
            return tradesWrapper.Trades;
        }

        /// <summary>
        /// Get a list of open trades (especially usefull for closing trades in FIFO style)
        /// </summary>
        /// <param name="instrument">Retrieve open trades for a specific instrument only</param>
        /// <param name="side">Retrieve open trades for a specific side only</param>
        /// <param name="units">Retrieve open trades with a specific number of units</param>
        /// <returns></returns>
        public async Task<List<Trade>> GetTrades(string instrument, OandaTypes.Side side, int units)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);

            TradesWrapper tradesWrapper = await Get<TradesWrapper>(routeParams, properties, _tradesRoute);

            return tradesWrapper.Trades.Where(x => x.Side == side && x.Units == units).OrderBy(x => x.Id).ToList();
        }

        #endregion

        #region GetTrade

        /// <summary>
        /// Get information on a specific trade
        /// </summary>
        /// <param name="tradeId">Trade id</param>
        /// <returns></returns>
        public async Task<Trade> GetTrade(long tradeId)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("tradeId", tradeId.ToString());

            Trade trade = await Get<Trade>(routeParams, null, _tradeRoute);
            return trade;
        }

        #endregion

        #region UpdateTrade

        /// <summary>
        /// Modify an existing trade
        /// </summary>
        /// <param name="tradeId">Trade id</param>
        /// <param name="stopLoss">Stop Loss value</param>
        /// <param name="takeProfit">Take Profit value</param>
        /// <param name="trailingStop">Trailing Stop distance in pips, up to one decimal place</param>
        /// <returns></returns>
        public async Task<Trade> UpdateTrade(long tradeId, float? stopLoss, float? takeProfit, int? trailingStop)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("tradeId", tradeId.ToString());

            Dictionary<string, string> properties = new Dictionary<string, string>();
            if (stopLoss != null) properties.Add("stopLoss", stopLoss.ToString());
            if (takeProfit != null) properties.Add("takeProfit", takeProfit.ToString());
            if (trailingStop != null) properties.Add("trailingStop", trailingStop.ToString());

            Trade trade = await Patch<Trade>(routeParams, properties, _tradeRoute);
            return trade;
        }

        #endregion

        #region CloseTrade

        /// <summary>
        /// Close an open trade
        /// </summary>
        /// <param name="tradeId">Trade id</param>
        /// <returns></returns>
        public async Task<TradeClosed> CloseTrade(long tradeId)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("tradeId", tradeId.ToString());

            TradeClosed tradeClosed = await Delete<TradeClosed>(routeParams, _tradeRoute);
            return tradeClosed;
        }

        /// <summary>
        /// Close an open trade by using th FIFO algorithem...
        /// </summary>
        /// <param name="instrument">Retrieve open trades for a specific instrument only</param>
        /// <param name="side">Retrieve open trades for a specific side only</param>
        /// <param name="units">Retrieve open trades with a specific number of units</param>
        /// <returns></returns>
        public async Task<TradeClosed> CloseTrade(string instrument, OandaTypes.Side side, int units)
        {
            TradeClosed tradeClosed;

            List<Trade> trades = await GetTrades(instrument, side, units);

            if (trades.Any())
            {
                // FIFO algorithem is implemented because GetTrades() is ordered by TradeId ascending...
                tradeClosed = await CloseTrade(trades.First().Id);
            }
            else
            {
                throw new Exception(string.Format("No trade closed because no trades were found for Instrument: {0}, Side: {1} and Units: {2}.", instrument, side, units));
            }

            return tradeClosed;
        }

        #endregion

    }
}
