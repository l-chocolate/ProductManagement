using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Database;

namespace ProductManagement.Infrastructure.Repositories
{
    public class Repository<TEntity>(ProductDbContext productDbContext) : IRepository<TEntity> where TEntity : Entity
    {
        private readonly ProductDbContext _productDbContext = productDbContext;
        public async Task<TEntity> Add(TEntity entity)
        {
            await _productDbContext.Set<TEntity>().AddAsync(entity);
            await _productDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(TEntity entity)
        {
            _productDbContext.Set<TEntity>().Remove(entity);
            await _productDbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> GetById(int id)
        {
            return await _productDbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _productDbContext.Set<TEntity>().Update(entity);
            await _productDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
