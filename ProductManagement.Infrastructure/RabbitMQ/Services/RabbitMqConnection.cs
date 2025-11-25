using Microsoft.Extensions.Options;
using ProductManagement.Infrastructure.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace ProductManagement.Infrastructure.RabbitMQ.Services
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly RabbitMqSettings _settings;
        public RabbitMqConnection(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
        }
        public async Task<IConnection> CreateConnectionAsync()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password,
                Port = _settings.Port
            };
            var retries = 5;

            while (retries > 0)
            {
                try
                {
                    return await factory.CreateConnectionAsync();
                }
                catch
                {
                    retries--;
                    await Task.Delay(2000);

                    if (retries == 0)
                        throw;
                }
            }

            throw new Exception("Failed to connect to RabbitMQ");
        }
    }
}
