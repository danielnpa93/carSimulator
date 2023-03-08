namespace DriverService.API.Domain.Repository
{
    public interface IMessageBrokerRepository<TKey, TValue>
    {
        Task ProduceAsync(TKey key, TValue message);
        void ConsumeAsync(CancellationToken cancellation, Action<TValue> handleMessage);
        void Dispose();
    }
}
