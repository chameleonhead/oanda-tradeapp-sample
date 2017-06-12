namespace Rabun.Oanda.Rest.Models
{
    public class OrderMarket: Order
    {
        public int Units { get; set; }
        public float TakeProfit { get; set; }
        public float StopLoss { get; set; }
        public float TrailingStop { get; set; }
    }
}