using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;
using System.Linq;

namespace Rabun.Oanda.Rest.Endpoints
{

    /// <summary>
    /// Rate endpoints
    /// </summary>
    public class RateEndpoints : Endpoint
    {
        private static readonly string _instrumentsRoute = "/v1/instruments";
        private static readonly string _priceRoute = "/v1/prices";
        private static readonly string _candleRoute = "/v1/candles";
        private readonly int _accountId;
        private readonly string _key;
        private AccountType _accountType;


        /// <summary>
        /// Initializes a new instance of the <see cref="RateEndpoints"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="accountId">The account identifier.</param>
        public RateEndpoints(string key, AccountType accountType, int accountId)
            : base(key, accountType)
        {
            _accountId = accountId;
            _key = key;
            _accountType = accountType;
        }

        #region GetInstruments

        /// <summary>
        /// Get an instrument list
        /// Get a list of tradeable instruments (currency pairs, CFDs, and commodities) that are available for trading with the account specified.
        /// </summary>
        /// <returns>Task of InstrumentModel List</returns>
        public async Task<List<InstrumentModel>> GetInstruments()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("accountId", _accountId);

            InstrumentWrapper result = await Get<InstrumentWrapper>(null, properties, _instrumentsRoute);
            return result.Instruments;

        }

        /// <summary>
        /// Get an instrument list
        /// Get a list of tradeable instruments (currency pairs, CFDs, and commodities) that are available for trading with the account specified.
        /// </summary>
        /// <param name="instruments">
        /// Optional An URL encoded (%2C) comma separated list of instruments that are to be returned in the response.
        /// If the instruments option is not specified, all instruments will be returned.
        /// </param>
        /// <returns>Task of InstrumentModel List</returns>
        public async Task<List<InstrumentModel>> GetInstruments(string instruments)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("accountId", _accountId);
            properties.Add("instruments", instruments);

            InstrumentWrapper result = await Get<InstrumentWrapper>(null, properties, _instrumentsRoute);
            return result.Instruments;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields">
        /// Optional An URL encoded (%2C) comma separated list of instrument fields that are to be returned in the response.
        /// The instrument field will be returned regardless of the input to this query parameter.
        /// Please see the Response Parameters section below for a list of valid values.
        /// </param>
        /// <param name="instruments">
        /// Optional An URL encoded (%2C) comma separated list of instruments that are to be returned in the response.
        /// If the instruments option is not specified, all instruments will be returned.
        /// </param>
        /// <returns></returns>
        public async Task<List<InstrumentModel>> GetInstruments(string fields, string instruments)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("accountId", _accountId);
            properties.Add("fields", fields);
            properties.Add("instruments", instruments);

            InstrumentWrapper result = await Get<InstrumentWrapper>(null, properties, _instrumentsRoute);
            return result.Instruments;

        }

        private class InstrumentWrapper
        {
            public List<InstrumentModel> Instruments { get; set; }
        }

        #endregion

        #region GetPrices

        /// <summary>
        /// Get current prices
        /// </summary>
        /// <param name="instruments">
        /// Required An URL encoded (%2C) comma separated list of instruments to fetch prices for.
        /// Values should be one of the available instrument from the /v1/instruments response.
        /// </param>
        /// <returns></returns>
        public async Task<List<Price>> GetPrices(string instruments)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instruments", instruments);

            PriceWrapper wrapper = await Get<PriceWrapper>(null, properties, _priceRoute);

            return wrapper.Prices;
        }



        private class PriceWrapper
        {
            public List<Price> Prices { get; set; }
        }

        #endregion

        #region GetMargins
        public async Task<int> GetAvailableUnitsToTrade(string instrument, OandaTypes.Side side, float maxMarginRatioToUse = 1)
        {
            AccountDetails accountDetails = await new AccountEndpoints(_key, _accountType).GetAccountDetails(_accountId);

            OandaTypes.TradeCurrency baseCurrency = (OandaTypes.TradeCurrency)Enum.Parse(typeof(OandaTypes.TradeCurrency), new string(instrument.ToCharArray().TakeWhile(x => x != '_').ToArray()), true);

            OandaTypes.AccountCurrency accountCurrency = accountDetails.AccountCurrency;

            float marginInstrumentPrice = 1;

            if (baseCurrency.ToString() != accountCurrency.ToString())
            {
                string marginInstrument = baseCurrency.ToString() + "_" + accountCurrency.ToString();

                if (!Enum.GetNames(typeof(OandaTypes.TradeCurrencyPair)).Contains(marginInstrument))
                {
                    marginInstrument = accountCurrency.ToString() + "_" + baseCurrency.ToString();
                }

                try
                {
                    if (side == OandaTypes.Side.buy)
                    {
                        marginInstrumentPrice = (await GetPrices(marginInstrument)).First().Ask;
                    }
                    else
                    {
                        marginInstrumentPrice = (await GetPrices(marginInstrument)).First().Bid;
                    }
                }
                catch
                {
                    return 0;
                }
            }

            return Convert.ToInt32(((accountDetails.MarginAvail * maxMarginRatioToUse) / accountDetails.MarginRate) / marginInstrumentPrice);
        }

        public async Task<float> GetMarginRequired(string instrument, int units)
        {
            AccountDetails accountDetails = await new AccountEndpoints(_key, _accountType).GetAccountDetails(_accountId);

            OandaTypes.TradeCurrency baseCurrency = (OandaTypes.TradeCurrency)Enum.Parse(typeof(OandaTypes.TradeCurrency), new string(instrument.ToCharArray().TakeWhile(x => x != '_').ToArray()), true);
            OandaTypes.AccountCurrency accountCurrency = accountDetails.AccountCurrency;

            float marginInstrumentPrice = 1;

            if (baseCurrency.ToString() != accountCurrency.ToString())
            {
                marginInstrumentPrice = (await GetPrices(baseCurrency.ToString() + "_" + accountCurrency.ToString())).First().Ask;
            }

            return Convert.ToSingle((marginInstrumentPrice * units) * accountDetails.MarginRate);
        }

        public async Task<float> GetMarginToAdd(string instrument, int units, float minMarginRatio)
        {
            AccountDetails accountDetails = await new AccountEndpoints(_key, _accountType).GetAccountDetails(_accountId); 

            float marginRequiredForTrade = await GetMarginRequired(instrument, units);

            var totalMargin = accountDetails.MarginUsed + accountDetails.MarginAvail;
            var marginAvailAfterTrade = accountDetails.MarginAvail - marginRequiredForTrade;

            return Convert.ToSingle((((minMarginRatio * totalMargin) - marginAvailAfterTrade) / (1 - minMarginRatio)) * accountDetails.MarginRate);
        }

        #endregion

        #region GetCandles

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleBidAsk>> GetCandles(string instrument)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("candleFormat", OandaTypes.CandleFormat.bidask);

            Candle<CandleBidAsk> result = await Get<Candle<CandleBidAsk>>(null, properties, _candleRoute);
            return result;
        }

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleBidAsk>> GetCandles(string instrument, OandaTypes.GranularityType granularity)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("granularity", granularity);
            properties.Add("candleFormat", OandaTypes.CandleFormat.bidask);

            Candle<CandleBidAsk> result = await Get<Candle<CandleBidAsk>>(null, properties, _candleRoute);
            return result;
        }

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <param name="count">
        /// The number of candles to return in the response.
        /// This parameter may be ignored by the server depending on the time range provided.
        /// See “Time and Count Semantics” below for a full description.
        /// If not specified, count will default to 500. The maximum acceptable value for count is 5000.
        /// Count should not be specified if both the start and end parameters are also specified.
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleBidAsk>> GetCandles(string instrument, OandaTypes.GranularityType granularity, int count)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("granularity", granularity);
            properties.Add("count", count);
            properties.Add("candleFormat", OandaTypes.CandleFormat.bidask);

            Candle<CandleBidAsk> result = await Get<Candle<CandleBidAsk>>(null, properties, _candleRoute);
            return result;
        }

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <param name="start">
        /// The start timestamp for the range of candles requested. The value specified must be in a valid datetime format.
        /// </param>
        /// <param name="end">
        /// The end timestamp for the range of candles requested. The value specified must be in a valid datetime format.
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleBidAsk>> GetCandles(string instrument, OandaTypes.GranularityType granularity, DateTime start, DateTime end)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("granularity", granularity);
            properties.Add("start", start.ToString("o"));
            properties.Add("end", end.ToString("o"));
            properties.Add("candleFormat", OandaTypes.CandleFormat.bidask);

            Candle<CandleBidAsk> result = await Get<Candle<CandleBidAsk>>(null, properties, _candleRoute);
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleMid>> GetCandlesMid(string instrument)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("candleFormat", OandaTypes.CandleFormat.midpoint);

            Candle<CandleMid> result = await Get<Candle<CandleMid>>(null, properties, _candleRoute);
            return result;
        }

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleMid>> GetCandlesMid(string instrument, OandaTypes.GranularityType granularity)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("granularity", granularity);
            properties.Add("candleFormat", OandaTypes.CandleFormat.midpoint);

            Candle<CandleMid> result = await Get<Candle<CandleMid>>(null, properties, _candleRoute);
            return result;
        }

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <param name="count">
        /// The number of candles to return in the response.
        /// This parameter may be ignored by the server depending on the time range provided.
        /// See “Time and Count Semantics” below for a full description.
        /// If not specified, count will default to 500. The maximum acceptable value for count is 5000.
        /// Count should not be specified if both the start and end parameters are also specified.
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleMid>> GetCandlesMid(string instrument, OandaTypes.GranularityType granularity, int count)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("granularity", granularity);
            properties.Add("count", count);
            properties.Add("candleFormat", OandaTypes.CandleFormat.midpoint);

            Candle<CandleMid> result = await Get<Candle<CandleMid>>(null, properties, _candleRoute);
            return result;
        }

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <param name="start">
        /// The start timestamp for the range of candles requested. The value specified must be in a valid datetime format.
        /// </param>
        /// <param name="end">
        /// The end timestamp for the range of candles requested. The value specified must be in a valid datetime format.
        /// </param>
        /// <returns></returns>
        public async Task<Candle<CandleMid>> GetCandlesMid(string instrument, OandaTypes.GranularityType granularity, DateTime start, DateTime end)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("granularity", granularity);
            properties.Add("start", start.ToString("o"));
            properties.Add("end", end.ToString("o"));
            properties.Add("candleFormat", OandaTypes.CandleFormat.midpoint);

            Candle<CandleMid> result = await Get<Candle<CandleMid>>(null, properties, _candleRoute);
            return result;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <param name="start">
        /// The start timestamp for the range of candles requested. The value specified must be in a valid datetime format.
        /// </param>
        /// <param name="end">
        /// The end timestamp for the range of candles requested. The value specified must be in a valid datetime format.
        /// </param>
        /// <param name="candleFormat">
        /// Candlesticks representation (about candestick representation).
        /// This can be one of the following: “midpoint” - Midpoint based candlesticks.
        /// “bidask” - Bid/Ask based candlesticks The default for candleFormat is “bidask” if the candleFormat parameter is not specified.
        /// </param>
        /// <param name="includeFirst">
        /// A boolean field which may be set to “true” or “false”.
        /// If it is set to “true”, the candlestick covered by the start timestamp will be returned.
        /// If it is set to “false”, this candlestick will not be returned.
        /// This field exists so clients may easily ensure that they can poll for all candles more recent than their last received candle.
        /// The default for includeFirst is “true” if the includeFirst parameter is not specified.
        /// </param>
        /// <param name="dailyAlignment">
        /// The hour of day used to align candles with hourly, daily, weekly, or monthly granularity.
        /// The value specified is interpretted as an hour in the timezone set through the
        /// alignmentTimezone parameter and must be an integer between 0 and 23.
        /// The default for dailyAlignment is 21 when Eastern Daylight Time is in effect and 22 when
        /// Eastern Standard Time is in effect. This corresponds to 17:00 local time in New York.
        /// </param>
        /// <param name="weeklyAlignment">
        /// Optional The timezone to be used for the dailyAlignment parameter.
        /// This parameter does NOT affect the returned timestamp, the start or end parameters, these will always be in UTC.
        /// The timezone format used is defined by the http://en.wikipedia.org/wiki/Tz_database, a full list of the
        /// timezones supported by the REST API can be found http://developer.oanda.com/docs/timezones.txt.
        /// </param>
        /// <returns></returns>
        public async Task<object> GetCandles(string instrument, OandaTypes.GranularityType? granularity, DateTime? start,
            DateTime? end, OandaTypes.CandleFormat? candleFormat, bool? includeFirst, byte? dailyAlignment,
            OandaTypes.WeeklyAlignment? weeklyAlignment)
        {
            Dictionary<string, object> properties = MakeCandle(instrument, granularity, null, start, end, candleFormat, includeFirst, dailyAlignment,
                weeklyAlignment);

            object candle = null;
            if (candleFormat == OandaTypes.CandleFormat.bidask)
            {
                candle = await Get<Candle<CandleBidAsk>>(null, properties, _candleRoute);
            }
            else if (candleFormat == OandaTypes.CandleFormat.midpoint)
            {
                candle = await Get<Candle<CandleMid>>(null, properties, _candleRoute);
            }

            return candle;
        }

        /// <summary>
        /// Retrieve instrument history
        /// Get historical information on an instrument
        /// </summary>
        /// <param name="instrument">
        /// Required Name of the instrument to retrieve history for. 
        /// The instrument should be one of the available instrument from the /v1/instruments response
        /// </param>
        /// <param name="granularity">
        /// Optional The time range represented by each candlestick. The value specified will determine the alignment of the first candlestick.
        /// </param>
        /// <param name="count">
        /// The number of candles to return in the response.
        /// This parameter may be ignored by the server depending on the time range provided.
        /// See “Time and Count Semantics” below for a full description.
        /// If not specified, count will default to 500. The maximum acceptable value for count is 5000.
        /// Count should not be specified if both the start and end parameters are also specified.
        /// </param>
        /// <param name="candleFormat">
        /// Candlesticks representation (about candestick representation).
        /// This can be one of the following: “midpoint” - Midpoint based candlesticks.
        /// “bidask” - Bid/Ask based candlesticks The default for candleFormat is “bidask” if the candleFormat parameter is not specified.
        /// </param>
        /// <param name="includeFirst">
        /// A boolean field which may be set to “true” or “false”.
        /// If it is set to “true”, the candlestick covered by the start timestamp will be returned.
        /// If it is set to “false”, this candlestick will not be returned.
        /// This field exists so clients may easily ensure that they can poll for all candles more recent than their last received candle.
        /// The default for includeFirst is “true” if the includeFirst parameter is not specified.
        /// </param>
        /// <param name="dailyAlignment">
        /// The hour of day used to align candles with hourly, daily, weekly, or monthly granularity.
        /// The value specified is interpretted as an hour in the timezone set through the
        /// alignmentTimezone parameter and must be an integer between 0 and 23.
        /// The default for dailyAlignment is 21 when Eastern Daylight Time is in effect and 22 when
        /// Eastern Standard Time is in effect. This corresponds to 17:00 local time in New York.
        /// </param>
        /// <param name="weeklyAlignment">
        /// Optional The timezone to be used for the dailyAlignment parameter.
        /// This parameter does NOT affect the returned timestamp, the start or end parameters, these will always be in UTC.
        /// The timezone format used is defined by the http://en.wikipedia.org/wiki/Tz_database, a full list of the
        /// timezones supported by the REST API can be found http://developer.oanda.com/docs/timezones.txt.
        /// </param>
        /// <returns></returns>
        /// <returns></returns>
        public async Task<object> GetCandles(string instrument, OandaTypes.GranularityType? granularity, int? count,
            OandaTypes.CandleFormat? candleFormat, bool? includeFirst, byte? dailyAlignment,
            OandaTypes.WeeklyAlignment? weeklyAlignment)
        {
            Dictionary<string, object> properties = MakeCandle(instrument, granularity, count, null, null, candleFormat, includeFirst, dailyAlignment, weeklyAlignment);

            object candle = null;
            if (candleFormat == OandaTypes.CandleFormat.bidask)
            {
                candle = await Get<Candle<CandleBidAsk>>(null, properties, _candleRoute);
            }
            else if (candleFormat == OandaTypes.CandleFormat.midpoint)
            {
                candle = await Get<Candle<CandleMid>>(null, properties, _candleRoute);
            }

            return candle;
        }

        private Dictionary<string, object> MakeCandle(string instrument, OandaTypes.GranularityType? granularity, int? count, DateTime? start, DateTime? end,
            OandaTypes.CandleFormat? candleFormat, bool? includeFirst, byte? dailyAlignment,
            OandaTypes.WeeklyAlignment? weeklyAlignment)
        {

            Dictionary<string, object> fields = new Dictionary<string, object>();

            if (string.IsNullOrWhiteSpace(instrument))
                throw new Exception("The instrument param can't be empty or null");

            fields.Add("instrument", instrument);

            if (granularity == null)
                granularity = OandaTypes.GranularityType.S5;
            fields.Add("granularity", granularity);

            if (count != null && start == null && end == null)
                fields.Add("count", count);

            if (start != null && end != null)
            {
                fields.Add("start", start.Value.ToString("o"));
                fields.Add("end", end.Value.ToString("o"));
            }
            else if (start != null)
            {
                fields.Add("start", start.Value.ToString("o"));
            }

            if (candleFormat != null)
                fields.Add("candleFormat", candleFormat.ToString());

            if (includeFirst == null)
                includeFirst = true;

            if (start != null)
                fields.Add("includeFirst", includeFirst.ToString().ToLower());

            if (dailyAlignment == null)
            {
                dailyAlignment = 22;
            }
            else if (dailyAlignment > 23)
                throw new Exception("The dailyAlignment must be between 0 and 23");

            fields.Add("dailyAlignment", dailyAlignment);

            if (weeklyAlignment == null)
                weeklyAlignment = OandaTypes.WeeklyAlignment.Friday;
            fields.Add("weeklyAlignment", weeklyAlignment.ToString());

            return fields;

        }

        #endregion


    }
}
