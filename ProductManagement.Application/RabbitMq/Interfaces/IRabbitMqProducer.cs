
namespace ProductManagement.Infrastructure.RabbitMQ.Interfaces
{
    public interface IRabbitMqProducer
    {
        Task SendMessageAsync(string message, string queueName);
    }
}
