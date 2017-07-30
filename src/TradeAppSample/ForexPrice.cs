using System;

namespace TradeAppSample
{
    public class ForexPrice : IComparable<ForexPrice>
    {
        public ForexPrice(decimal ask, decimal bid)
        {
            if (ask <= bid)
                throw new ArgumentException();
            Ask = ask;
            Bid = bid;
        }

        public decimal Ask { get; }
        public decimal Bid { get; }
        public decimal Spread
        {
            get { return Ask - Bid; }
        }

        public int CompareTo(ForexPrice other)
        {
            var thisTotal = Ask + Bid;
            var otherTotal = other.Ask + other.Bid;

            if (thisTotal == otherTotal)
                return 0;

            if (thisTotal > otherTotal)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}