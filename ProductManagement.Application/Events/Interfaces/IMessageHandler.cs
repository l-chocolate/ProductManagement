
namespace ProductManagement.Application.Events.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task HandleMessageAsync(T message, string payload);
        Task HandleErrorAsync(string payload, Exception exception);
    }
}
