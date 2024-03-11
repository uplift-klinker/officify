using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Officify.Core.Common.Validation;

public class FluentValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var failures = await ExecuteValidatorsAsync(request, cancellationToken)
            .ConfigureAwait(false);
        if (failures.Length != 0)
            throw new ValidationException(failures);

        return await next().ConfigureAwait(false);
    }

    private async Task<ValidationFailure[]> ExecuteValidatorsAsync(
        TRequest request,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);
        var tasks = validators.Select(v => v.ValidateAsync(context, cancellationToken));
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);

        return results.Where(r => !r.IsValid).SelectMany(r => r.Errors).ToArray();
    }
}
