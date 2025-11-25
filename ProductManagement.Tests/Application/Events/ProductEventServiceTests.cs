using Moq;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.RabbitMQ.Interfaces;
using System.Text.Json;

namespace ProductManagement.Tests.Application.Events;
public class ProductEventServiceTests
{
    [Fact]
    public async Task Should_Publish_ProductCreated_Event()
    {
        var producerMock = new Mock<IRabbitMqProducer>();
        var service = new ProductEventService(producerMock.Object);

        Product product = new Product
        {
            Name = "Café",
            Category = "Bebida" ,
            CreatedAt = DateTime.UtcNow,
            UnitCost = 19.90
        };

        await service.PublishCreatedProductAsync(product);
        string payload = JsonSerializer.Serialize(product);

        producerMock.Verify(x => x.SendMessageAsync(payload, "product.created"), Times.Once);
    }
}
