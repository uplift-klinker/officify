using MediatR;

namespace Officify.Core.Common.Queries;

public interface IQuery<out TResult> : IRequest<TResult>;
