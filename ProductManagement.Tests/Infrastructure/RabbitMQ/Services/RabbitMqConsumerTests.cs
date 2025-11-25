using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.RabbitMQ.Interfaces;
using ProductManagement.Infrastructure.RabbitMQ.Services;
using System.Text.Json;

namespace ProductManagement.Tests.Application.RabbitMq.Services
{
    public class RabbitMqConsumerTests
    {
        [Fact]
        public async Task Should_Call_Handler_When_Valid_Message_Arrives()
        {
            var handlerMock = new Mock<IMessageHandler<Product>>();

            var connectionMock = new Mock<IRabbitMqConnection>();
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();

            var consumer = new RabbitMqConsumer<Product>(
                connectionMock.Object,
                scopeFactoryMock.Object);

            var product = new Product
            {
                Id = 1,
                Name = "Café",
                Category = "Bebida",
                CreatedAt = DateTime.UtcNow,
                UnitCost = 19.90
            };

            string json = JsonSerializer.Serialize(product);

            await consumer.ProcessMessageForTestsAsync(json, handlerMock.Object);

            handlerMock.Verify(
                h => h.HandleMessageAsync(
                    It.Is<Product>(p => p.Id == product.Id && p.Name == product.Name),
                    json),
                Times.Once);
        }
        [Fact]
        public async Task Should_Call_HandleErrorAsync_When_Handler_Throws_Exception()
        {
            var handlerMock = new Mock<IMessageHandler<Product>>();

            handlerMock
                .Setup(h => h.HandleMessageAsync(It.IsAny<Product>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Erro de teste"));

            var connectionMock = new Mock<IRabbitMqConnection>();
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();

            var consumer = new RabbitMqConsumer<Product>(
                connectionMock.Object,
                scopeFactoryMock.Object);

            string payload = @"{ ""Id"": 1, ""Name"": ""Café"", ""Category"": ""Bebida"", ""UnitCost"": 10.0 }";

            await consumer.ProcessMessageForTestsAsync(payload, handlerMock.Object);

            handlerMock.Verify(
                h => h.HandleErrorAsync(
                    payload,
                    It.IsAny<Exception>()
                ),
                Times.Once);
        }
        [Fact]
        public async Task Should_Call_HandleErrorAsync_When_Json_Is_Invalid()
        {
            var handlerMock = new Mock<IMessageHandler<Product>>();

            var connectionMock = new Mock<IRabbitMqConnection>();
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();

            var consumer = new RabbitMqConsumer<Product>(
                connectionMock.Object,
                scopeFactoryMock.Object);

            string invalidJson = "{ inválido json total }";

            await consumer.ProcessMessageForTestsAsync(invalidJson, handlerMock.Object);

            handlerMock.Verify(
                h => h.HandleErrorAsync(
                    invalidJson,
                    It.IsAny<Exception>()
                ),
                Times.Once);
        }
    }
}