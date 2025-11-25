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
            var retries = 5;

            while (retries > 0 && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _consumer.ReceiveMessagesAsync("product.created", stoppingToken);
                    break;
                }
                catch (Exception)
                {
                    retries--;
                    await Task.Delay(2000, stoppingToken);

                    if (retries == 0)
                        throw;
                }
            }
        }
    }
}
