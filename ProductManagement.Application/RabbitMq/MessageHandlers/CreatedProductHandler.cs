using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Application.RabbitMq.MessageHandlers
{
    public class CreatedProductHandler(IProductEventRepository productEventRepository) : IMessageHandler<Product>
    {
        private readonly IProductEventRepository _productEventRepository = productEventRepository;

        public async Task HandleErrorAsync(string payload, Exception exception)
        {
            await _productEventRepository.Add(new ProductEvent()
            {
                Payload = payload,
                ErrorMessage = exception.Message,
                ProcessedAt = DateTime.UtcNow
            });
        }

        public async Task HandleMessageAsync(Product message, string payload)
        {
            await _productEventRepository.Add(new ProductEvent()
            {
                ProductId = message.Id,
                Payload = payload,
                ProcessedAt = DateTime.UtcNow
            });
        }
    }
}
