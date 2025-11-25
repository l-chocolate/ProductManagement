
namespace ProductManagement.Infrastructure.RabbitMQ.Interfaces
{
    public interface IRabbitMqConsumer<T>
    {
        Task ReceiveMessagesAsync(string queueName, CancellationToken cancellationToken);
    }
}
