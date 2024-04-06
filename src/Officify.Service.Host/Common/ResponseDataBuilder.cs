using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Officify.Core.Common;
using Officify.Core.Common.Commands;
using Officify.Core.Common.Exceptions;
using Officify.Core.Common.Queries;

namespace Officify.Service.Host.Common;

public class ResponseDataBuilder(IMessageBus messageBus)
{
    public ResponseDataRequestBuilder UseRequest(HttpRequestData request)
    {
        return new ResponseDataRequestBuilder(messageBus, request);
    }
}

public class ResponseDataRequestBuilder(IMessageBus messageBus, HttpRequestData request)
{
    public async Task<HttpResponseData> ExecuteAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    )
    {
        var result = await TryExecuteAsync(query, cancellationToken);
        return await CreateResponseFromResultAsync(result, cancellationToken);
    }

    public async Task<HttpResponseData> ExecuteAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        var result = await TryExecuteAsync(command, cancellationToken);
        return await CreateResponseFromResultAsync(result, cancellationToken);
    }

    private async Task<HttpResponseData> CreateResponseFromResultAsync<TResult>(
        TResult? result,
        CancellationToken cancellationToken
    )
    {
        var response = request.CreateResponse();
        response.StatusCode = result == null ? HttpStatusCode.NotFound : HttpStatusCode.OK;
        if (result != null)
            await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }

    private async Task<TResult?> TryExecuteAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    )
    {
        return await TryExecuteAsync(() => messageBus.ExecuteAsync(query, cancellationToken));
    }

    private async Task<TResult?> TryExecuteAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        return await TryExecuteAsync(() => messageBus.ExecuteAsync(command, cancellationToken));
    }

    private static async Task<TResult?> TryExecuteAsync<TResult>(Func<Task<TResult>> func)
    {
        try
        {
            return await func.Invoke();
        }
        catch (EntityNotFoundException)
        {
            return default;
        }
        catch (TaskCanceledException)
        {
            return default;
        }
    }
}
