namespace Rabun.Oanda.Rest.Models
{
    public static class OandaTypes
    {
        public enum GranularityType
        {
            S5,
            S10,
            S15,
            S30,
            M1,
            M2,
            M3,
            M5,
            M10,
            M15,
            M30,
            H1,
            H2,
            H3,
            H4,
            H6,
            H8,
            H12,
            D,
            W,
            M
        }

        public enum Side
        {
            buy,
            sell
        }

        public enum OrderType
        {
            limit,
            stop,
            marketIfTouched,
            market
        }

        public enum TransactionType
        {
            MARKET_ORDER_CREATE,
            STOP_ORDER_CREATE,
            LIMIT_ORDER_CREATE,
            MARKET_IF_TOUCHED_ORDER_CREATE,
            ORDER_UPDATE,
            ORDER_CANCEL,
            ORDER_FILLED,
            TRADE_UPDATE,
            TRADE_CLOSE,
            MIGRATE_TRADE_OPEN,
            MIGRATE_TRADE_CLOSE,
            STOP_LOSS_FILLED,
            TAKE_PROFIT_FILLED,
            TRAILING_STOP_FILLED,
            MARGIN_CALL_ENTER,
            MARGIN_CALL_EXIT,
            MARGIN_CLOSEOUT,
            SET_MARGIN_RATE,
            TRANSFER_FUNDS,
            DAILY_INTEREST,
            FEE
        }

        public enum Reason
        {
            CLIENT_REQUEST,
            MIGRATION,
            REPLACES_ORDER,
            ORDER_FILLED
        }

        public enum CandleFormat
        {
            midpoint,
            bidask
        }

        public enum WeeklyAlignment
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        public enum AccountCurrency
        {
            EUR,
            USD,
            SGD,
            CHF,
            CAD,
            GBP,
            AUD,
            HKD,
            JPY
        }

        public enum TradeCurrency
        {
            EUR,
            USD,
            SGD,
            CHF,
            CAD,
            GBP,
            JPY,
            AUD,
            ZAR,
            HKD,
            NZD,
            CZK,
            DKK,
            HUF,
            NOK,
            SEK,
            SAR,
            TRY,
            CNH,
            MXN,
            PLN,
            THD
        }

        public enum TradeCurrencyPair
        {
            AUD_JPY,
            AUD_USD,	
            EUR_AUD,	
            EUR_CHF,	
            EUR_GBP,	
            EUR_JPY,	
            EUR_USD,	
            GBP_CHF,	
            GBP_JPY,	
            GBP_USD,	
            NZD_USD,	
            USD_CAD,	
            USD_CHF,	
            USD_JPY,
            AUD_CAD,
            AUD_CHF,
            AUD_HKD,
            AUD_NZD,
            AUD_SGD,
            CAD_CHF,
            CAD_HKD,
            CAD_JPY,
            CAD_SGD,
            CHF_HKD,
            CHF_JPY,
            CHF_ZAR,
            EUR_CAD,
            EUR_CZK,
            EUR_DKK,
            EUR_HKD,
            EUR_HUF,
            EUR_NOK,
            EUR_NZD,
            EUR_PLN,
            EUR_SEK,
            EUR_SGD,
            EUR_TRY,
            EUR_ZAR,
            GBP_AUD,
            GBP_CAD,
            GBP_HKD,
            GBP_NZD,
            GBP_PLN,
            GBP_SGD,
            GBP_ZAR,
            HKD_JPY,
            NZD_CAD,
            NZD_CHF,
            NZD_HKD,
            NZD_JPY,
            NZD_SGD,
            SGD_CHF,
            SGD_HKD,
            SGD_JPY,
            TRY_JPY,
            USD_CNH,
            USD_CZK,
            USD_DKK,
            USD_HKD,
            USD_HUF,
            USD_INR,
            USD_MXN,
            USD_NOK,
            USD_PLN,
            USD_SAR,
            USD_SEK,
            USD_SGD,
            USD_THB,
            USD_TRY,
            USD_ZAR,
            ZAR_JPY  
        }
    }
}
