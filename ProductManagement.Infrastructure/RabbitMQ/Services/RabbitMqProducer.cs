using ProductManagement.Infrastructure.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductManagement.Infrastructure.RabbitMQ.Services
{
    public class RabbitMqProducer(IRabbitMqConnection connection) : IRabbitMqProducer
    {
        IRabbitMqConnection _connection = connection;
        public async Task SendMessageAsync(string message, string queueName)
        {
            using IConnection conn = await _connection.CreateConnectionAsync();
            using IChannel channel = await conn.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            byte[] body = System.Text.Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "",
                                 routingKey: queueName,
                                 body: body);
        }
    }
}
