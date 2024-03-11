using MediatR;
using Officify.Core.Common.Commands;
using Officify.Core.Common.Events;
using Officify.Core.Common.Queries;

namespace Officify.Core.Common;

public interface IMessageBus
{
    Task ExecuteAsync(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> ExecuteAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    );
    Task<TResult> ExecuteAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    );
    Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);
}

public class MessageBus(IMediator mediator) : IMessageBus
{
    public async Task ExecuteAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        await mediator.Send(command, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        return await mediator.Send(command, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    )
    {
        return await mediator.Send(query, cancellationToken).ConfigureAwait(false);
    }

    public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        await mediator.Publish(@event, cancellationToken);
    }
}
