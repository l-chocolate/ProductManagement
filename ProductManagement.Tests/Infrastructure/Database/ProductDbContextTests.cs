using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Database;
using ProductManagement.Infrastructure.Repositories;
using System;

namespace ProductManagement.Tests.Infrastructure.Database
{
    public class ProductDbContextTests
    {
        [Fact]
        public async Task Should_Save_ProductEvent_InMemory()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase("product_management_testdb")
                .Options;

            var context = new ProductDbContext(options);

            var repo = new ProductEventRepository(context);

            await repo.Add(new ProductEvent
            {
                ProductId = 1,
                Payload = "{}",
                ProcessedAt = DateTime.UtcNow
            });

            Assert.Equal(1, context.ProductEvents.Count());
        }

    }
}
