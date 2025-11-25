using RabbitMQ.Client;

namespace ProductManagement.Infrastructure.RabbitMQ.Interfaces
{
    public interface IRabbitMqConnection
    {
        Task<IConnection> CreateConnectionAsync();
    }
}
