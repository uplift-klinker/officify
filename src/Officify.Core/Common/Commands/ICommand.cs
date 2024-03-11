using MediatR;

namespace Officify.Core.Common.Commands;

public interface ICommand : ICommand<Unit>;

public interface ICommand<out TResult> : IRequest<TResult>;
