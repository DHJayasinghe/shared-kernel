using SharedKernel.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Interfaces.EventBus;

public interface IEventBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : BaseDomainEvent;
    Task SendAsync<T>(T message, CancellationToken cancellationToken = default) where T : BaseMessage;

    Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : BaseRpcRequest;
    Task RespondAsync<TResponse>(TResponse response, string replyTo, string replyToSessionId) where TResponse : class;
}
