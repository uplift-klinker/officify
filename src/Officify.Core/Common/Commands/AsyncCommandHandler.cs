using MediatR;

namespace Officify.Core.Common.Commands;

public abstract class AsyncCommandHandler<TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand, IRequest
{
    public async Task Handle(TCommand request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken).ConfigureAwait(false);
    }

    protected abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
