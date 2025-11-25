using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Events.Interfaces
{
    public interface IProductEventService
    {
        Task PublishCreatedProductAsync(Product product);
    }
}
