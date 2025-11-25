using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.RabbitMQ.Interfaces;

public class ProductEventService(IRabbitMqProducer producer) : IProductEventService
{
    private readonly IRabbitMqProducer _producer = producer;

    public async Task PublishCreatedProductAsync(Product product)
    {
        string productPayload = System.Text.Json.JsonSerializer.Serialize(product);
        await _producer.SendMessageAsync(productPayload, "product.created");
    }

}