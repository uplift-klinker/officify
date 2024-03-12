using Microsoft.AspNetCore.Mvc;
using Officify.Core.Common;
using Officify.Core.Common.Commands;
using Officify.Core.Common.Exceptions;
using Officify.Core.Common.Queries;

namespace Officify.Service.Host.Common;

public class MessageBusController(IMessageBus messageBus)
{
    protected async Task<IActionResult> ExecuteAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var result = await messageBus
                .ExecuteAsync(command, cancellationToken)
                .ConfigureAwait(false);
            return new JsonResult(result);
        }
        catch (EntityNotFoundException e)
        {
            return new NotFoundResult();
        }
    }

    protected async Task<IActionResult> ExecuteAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var result = await messageBus
                .ExecuteAsync(query, cancellationToken)
                .ConfigureAwait(false);
            return result == null ? new NotFoundResult() : new JsonResult(result);
        }
        catch (EntityNotFoundException e)
        {
            return new NotFoundResult();
        }
    }
}
