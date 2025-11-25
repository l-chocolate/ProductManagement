using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Database;

namespace ProductManagement.Infrastructure.Repositories
{
    public class ProductEventRepository : Repository<ProductEvent>, IProductEventRepository
    {
        public ProductEventRepository(ProductDbContext productDbContext) : base(productDbContext)
        {
        }
    }
}
