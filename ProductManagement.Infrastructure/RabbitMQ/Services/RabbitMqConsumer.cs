using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Infrastructure.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text.Json;

namespace ProductManagement.Infrastructure.RabbitMQ.Services
{
    public class RabbitMqConsumer<T>(IRabbitMqConnection connection, IServiceScopeFactory scopeFactory) : IRabbitMqConsumer<T>
    {
        IRabbitMqConnection _connection = connection;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        public async Task ReceiveMessagesAsync(string queueName, CancellationToken cancellationToken)
        {
            using IConnection conn = await _connection.CreateConnectionAsync();
            using IChannel channel = await conn.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            using IServiceScope scope = _scopeFactory.CreateScope();
            IMessageHandler<T> messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler<T>>();

            AsyncEventingBasicConsumer eventConsumer = new AsyncEventingBasicConsumer(channel);
            eventConsumer.ReceivedAsync += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string json = System.Text.Encoding.UTF8.GetString(body);
                try
                {
                    T message = JsonSerializer.Deserialize<T>(json)!;
                    await messageHandler.HandleMessageAsync(message, json);
                }
                catch (OperationInterruptedException ex)
                {
                    await messageHandler.HandleErrorAsync(json, ex);
                }
                catch (BrokerUnreachableException ex)
                {
                    await messageHandler.HandleErrorAsync(json, ex);
                }
                catch (Exception ex)
                {
                    await messageHandler.HandleErrorAsync(json, ex);
                }
            };

            await channel.BasicConsumeAsync(queue: queueName,
                                         autoAck: true,
                                         consumer: eventConsumer,
                                         cancellationToken: cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }

        }
        public async Task ProcessMessageForTestsAsync(string json, IMessageHandler<T> messageHandler)
        {
            try
            {
                T message = JsonSerializer.Deserialize<T>(json)!;
                await messageHandler.HandleMessageAsync(message, json);
            }
            catch (Exception ex)
            {
                await messageHandler.HandleErrorAsync(json, ex);
            }
        }
    }
}
