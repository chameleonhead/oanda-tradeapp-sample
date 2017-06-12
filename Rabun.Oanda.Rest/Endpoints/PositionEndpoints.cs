using System.Collections.Generic;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;

namespace Rabun.Oanda.Rest.Endpoints
{
    /// <summary>
    /// Position endpoints
    /// </summary>
    public class PositionEndpoints : Endpoint
    {

        private static string _positionsRoute = "/v1/accounts/:accountId/positions";
        private static string _positionRoute = "/v1/accounts/:accountId/positions/:instrument";
        private readonly int _accountId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionEndpoints"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="accountId">The account identifier.</param>
        public PositionEndpoints(string key, AccountType accountType, int accountId)
            : base(key, accountType)
        {
            _accountId = accountId;
        }

        #region GetPositions

        private class PositionsWrapper
        {
            public List<Position> Positions { get; set; }
        }

        /// <summary>
        /// Get a list of all open positions
        /// </summary>
        /// <returns>list of positions</returns>
        public async Task<List<Position>> GetPositions()
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            PositionsWrapper result = await Get<PositionsWrapper>(routeParams, null, _positionsRoute);
            return result.Positions;
        }

        /// <summary>
        /// Get the position for an instrument
        /// </summary>
        /// <param name="instrument">instrument</param>
        /// <returns>position</returns>
        public async Task<Position> GetPosition(string instrument)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("instrument", instrument);

            Position result = await Get<Position>(routeParams, null, _positionRoute);
            return result;
        }

        /// <summary>
        /// Close an existing position
        /// </summary>
        /// <param name="instrument">instrument instrument</param>
        /// <returns></returns>
        public async Task<PositionClosed> ClosePosition(string instrument)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("instrument", instrument);

            PositionClosed positionClosed = await Delete<PositionClosed>(routeParams, _positionRoute);
            return positionClosed;
        }

        #endregion
    }
}
