using Microsoft.AspNetCore.Mvc;
using Officify.Core.Common;
using Officify.Core.Common.Commands;
using Officify.Core.Common.Exceptions;
using Officify.Core.Common.Queries;

namespace Officify.Service.Host.Common;

[ApiController]
public class MessageBusController(IMessageBus messageBus) : Controller
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
            return Json(result);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
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
            return result == null ? NotFound() : Json(result);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
    }
}
