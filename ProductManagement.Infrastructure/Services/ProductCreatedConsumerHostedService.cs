using Microsoft.Extensions.Hosting;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.RabbitMQ.Interfaces;

namespace ProductManagement.Infrastructure.Services
{
    public class ProductCreatedConsumerHostedService : BackgroundService
    {
        private readonly IRabbitMqConsumer<Product> _consumer;
        public ProductCreatedConsumerHostedService(IRabbitMqConsumer<Product> consumer)
        {
            _consumer = consumer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ReceiveMessagesAsync(
                queueName: "product.created",
                cancellationToken: stoppingToken
            );
        }
    }
}
