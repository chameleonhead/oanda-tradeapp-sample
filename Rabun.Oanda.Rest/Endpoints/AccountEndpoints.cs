using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;
using System.Threading;

namespace Rabun.Oanda.Rest.Endpoints
{
    /// <summary>
    /// Account endpoints
    /// </summary>
    public class AccountEndpoints : Endpoint
    {
        private readonly string _accountsRoute = "/v1/accounts";
        private readonly string _accountDetailRoute = "/v1/accounts/:accountId";

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountEndpoints"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="accountType">Type of the account.</param>
        public AccountEndpoints(string key, AccountType accountType)
            : base(key, accountType)
        {

        }

        #region GetAccounts

        private class AccountsWrapper
        {
            public List<Account> Accounts { get; set; }
        }

        /// <summary>
        /// This will return all accounts given access token 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> GetAccounts()
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();

            AccountsWrapper accountsWrapper = await Get<AccountsWrapper>(routeParams, null, _accountsRoute);
            return accountsWrapper.Accounts;
        }

        #endregion

        #region GetAccountDetails

        /// <summary>
        /// Get information of a given account
        /// </summary>
        /// <param name="accountId">AccountId of the given account</param>
        /// <returns></returns>
        public async Task<AccountDetails> GetAccountDetails(int accountId)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", accountId.ToString());

            AccountDetails accountDetails = await Get<AccountDetails>(routeParams, null, _accountDetailRoute);
            return accountDetails;
        }

        #endregion
    }
}
