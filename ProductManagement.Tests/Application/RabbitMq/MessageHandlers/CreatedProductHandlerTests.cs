using Moq;
using ProductManagement.Application.RabbitMq.MessageHandlers;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using System.Text.Json;

namespace ProductManagement.Tests.Application.RabbitMq.MessageHandlers
{
    public class CreatedProductHandlerTests
    {
        [Fact]
        public async Task Should_Save_Event_When_Handling_ProductCreated()
        {
            Mock<IProductEventRepository> repo = new Mock<IProductEventRepository>();

            CreatedProductHandler handler = new CreatedProductHandler(repo.Object);

            Product product = new Product
            {
                Id = 1,
                Name = "Café",
                Category = "Bebida",
                UnitCost = 19.9,
                CreatedAt = DateTime.Now
            };
            string payload = JsonSerializer.Serialize(product);

            await handler.HandleMessageAsync(product, payload);

            repo.Verify(x => x.Add(It.IsAny<ProductEvent>()), Times.Once);
        }

    }
}
