using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rabun.Oanda.Rest.Base;

namespace Rabun.Oanda.Rest.Factories
{
    /// <summary>
    /// Default factory for all endpoints
    /// </summary>
    public class DefaultFactory
    {
        private readonly string _key;
        private readonly AccountType _accountType;
        private readonly int _accountId;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="key">Oanda account private key</param>
        /// <param name="accountType">Account type (practice or real)</param>
        /// <param name="accountId">Account Id</param>
        public DefaultFactory(string key, AccountType accountType, int accountId)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new NullReferenceException("Key don't be null or empry");

            _key = key;
            _accountType = accountType;
            _accountId = accountId;
        }

        /// <summary>
        /// Make endpoints instance. Use generic method.
        /// </summary>
        /// <typeparam name="T">Endpoint type</typeparam>
        /// <returns></returns>
        public T GetEndpoint<T>() where T : Endpoint
        {
            Type t = typeof (T);

            return (T) Activator.CreateInstance(t, _key, _accountType, _accountId);
        }
    }
}
