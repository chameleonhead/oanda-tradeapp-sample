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

        public override int GetHashCode()
        {
            return (Ask + Bid).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ForexPrice)) return false;
            var price = obj as ForexPrice;
            return Ask == price.Ask && Bid == price.Bid;
        }

        public static bool operator ==(ForexPrice p1, ForexPrice p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(ForexPrice p1, ForexPrice p2)
        {
            return !p1.Equals(p2);
        }

        public static bool operator <(ForexPrice p1, ForexPrice p2)
        {
            return p1.CompareTo(p2) < 0;
        }

        public static bool operator >(ForexPrice p1, ForexPrice p2)
        {
            return p1.CompareTo(p2) > 0;
        }
    }
}