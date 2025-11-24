using ProductManagement.Domain.Entities;

namespace ProductManagement.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(TEntity entity);
        Task<TEntity?> GetById(int id);
    }
}
