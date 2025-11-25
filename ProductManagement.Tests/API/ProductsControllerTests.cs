using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagement.API.Controllers;
using ProductManagement.API.DTOs;
using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Tests.API
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly Mock<IProductEventService> _eventMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _eventMock = new Mock<IProductEventService>();

            _controller = new ProductsController(_repoMock.Object, _eventMock.Object);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreated_WhenProductIsValid()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Café",
                CategoryName = "Bebidas",
                UnitCost = 10.5
            };

            _repoMock.Setup(r => r.Add(It.IsAny<Product>()))
                     .Callback<Product>(p => p.Id = 10)
                     .ReturnsAsync((Product p) =>
                     {
                         p.Id = 10;
                         return p;
                     });

            // Act
            var result = await _controller.PostProduct(request);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetProduct), created.ActionName);

            var product = Assert.IsType<Product>(created.Value);
            Assert.Equal("Café", product.Name);
            Assert.Equal("Bebidas", product.Category);

            _repoMock.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
            _eventMock.Verify(e => e.PublishCreatedProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct_WhenExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 5,
                Name = "Teclado",
                Category = "Eletrônicos",
                UnitCost = 100,
                CreatedAt = DateTime.UtcNow
            };

            _repoMock.Setup(r => r.GetById(5)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(5);

            // Assert
            var ok = Assert.IsType<ProductResponse>(result.Value);
            Assert.Equal("Teclado", ok.Name);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _repoMock.Setup(r => r.GetById(99)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetProduct(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutProduct_Updates_WhenExists()
        {
            // Arrange
            var existing = new Product
            {
                Id = 7,
                Name = "Mouse",
                Category = "Eletrônicos",
                UnitCost = 50,
                CreatedAt = DateTime.UtcNow
            };

            _repoMock.Setup(r => r.GetById(7)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.Update(existing)).ReturnsAsync(existing);

            var request = new UpdateProductRequest
            {
                Name = "Mouse Gamer",
                CategoryName = "Acessórios",
                UnitCost = 80
            };

            // Act
            var result = await _controller.PutProduct(request, 7);

            // Assert
            var response = Assert.IsType<ProductResponse>(result.Value);
            Assert.Equal("Mouse Gamer", response.Name);
            Assert.Equal("Acessórios", response.CategoryName);
            Assert.Equal(80, response.UnitCost);

            _repoMock.Verify(r => r.Update(existing), Times.Once);
        }

        [Fact]
        public async Task PutProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _repoMock.Setup(r => r.GetById(70)).ReturnsAsync((Product?)null);

            var request = new UpdateProductRequest { Name = "Teste" };

            // Act
            var result = await _controller.PutProduct(request, 70);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenDeleted()
        {
            // Arrange
            var existing = new Product { Id = 10, Name = "Monitor", Category = "Eletrônicos", CreatedAt = DateTime.UtcNow };

            _repoMock.Setup(r => r.GetById(10)).ReturnsAsync(existing);

            // Act
            var result = await _controller.DeleteProduct(10);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _repoMock.Verify(r => r.Delete(existing), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _repoMock.Setup(r => r.GetById(33)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.DeleteProduct(33);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
