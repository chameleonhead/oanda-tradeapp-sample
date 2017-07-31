namespace TradeAppSample.Shared
{
    public interface IEventHandler<T>
        where T : IEvent
    {
        void Handle(T evt);
    }
}
