using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;

namespace Rabun.Oanda.Rest.Endpoints
{
    /// <summary>
    /// Transaction endpoints
    /// </summary>
    public class TransactionEndpoints : Endpoint
    {
        private readonly string _transactionsRoute = "/v1/accounts/:accountId/transactions";
        private readonly int _accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionEndpoints"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="accountId">The account identifier.</param>
        public TransactionEndpoints(string key, AccountType accountType, int accountId)
            : base(key, accountType)
        {
            _accountId = accountId;
        }

        /// <summary>
        /// Get transaction history
        /// </summary>
        /// <param name="maxId">The first transaction to get. The server will return transactions with id less than or equal to this, in descending order</param>
        /// <param name="minId">The last transaction to get. The server will return transactions with id greater or equal to this, in descending order</param>
        /// <param name="count">The maximum number of transactions to return. The maximum value that can be specified is 500. By default, if count is not specified, a maximum of 50 transactions will be fetched. Note: Transactions requests with the count parameter specified is rate limited to 1 per every 60 seconds</param>
        /// <param name="instrument">Retrieve transactions for a specific instrument only</param>
        /// <param name="ids">An URL encoded comma (%2C) separated list of transaction ids to retrieve. Maximum number of ids: 50. No other parameter may be specified with the ids parameter</param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetTransactions(long? maxId, long? minId, int? count, string instrument, string ids)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(instrument)) properties.Add("instrument", instrument);
            if (count != null) properties.Add("count", count);
            if (maxId != null) properties.Add("maxId", maxId);
            if (minId != null) properties.Add("minId", minId);
            if (!string.IsNullOrWhiteSpace(ids)) properties.Add("ids", ids);


            List<Transaction> transactions = new List<Transaction>();
            JObject result = await Get<JObject>(routeParams, properties, _transactionsRoute);

            int countResult = result["transactions"].Count();
            for (int i = 0; i < countResult; i++)
            {
                OandaTypes.TransactionType type;
                Enum.TryParse(result["transactions"][i]["type"].Value<string>(), out type);

                switch (type)
                {
                    case OandaTypes.TransactionType.MARKET_ORDER_CREATE:
                        {

                            Transaction transaction = result["transactions"][i].ToObject<TransactionMarketOrderCreate>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.LIMIT_ORDER_CREATE:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionLimitOrderCreate>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.ORDER_CANCEL:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionOrderCancel>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.MARKET_IF_TOUCHED_ORDER_CREATE:
                        {

                            Transaction transaction = result["transactions"][i].ToObject<TransactionMarketIfTouchedCreate>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.ORDER_UPDATE:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionOrderUpdate>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.ORDER_FILLED:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionOrderFilled>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.TRADE_UPDATE:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionTradeUpdate>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.TRADE_CLOSE:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionTradeClose>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.MIGRATE_TRADE_OPEN:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionMigrateTradeOpen>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.MIGRATE_TRADE_CLOSE:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionMigrateTradeClose>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.STOP_LOSS_FILLED:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionStopLossField>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.TAKE_PROFIT_FILLED:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionTakeProfitField>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.TRAILING_STOP_FILLED:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionTrailingStopField>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.MARGIN_CALL_ENTER:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionMarginCallEnter>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.MARGIN_CALL_EXIT:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionMarginCallExit>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.MARGIN_CLOSEOUT:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionMarginCloseOut>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.SET_MARGIN_RATE:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionSatMarginRate>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.TRANSFER_FUNDS:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionTransferFounds>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.DAILY_INTEREST:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionDailyInterest>();
                            transactions.Add(transaction);
                            break;
                        }
                    case OandaTypes.TransactionType.FEE:
                        {
                            Transaction transaction = result["transactions"][i].ToObject<TransactionFee>();
                            transactions.Add(transaction);
                            break;
                        }
                }
            }

            return transactions;

        }

    }
}
