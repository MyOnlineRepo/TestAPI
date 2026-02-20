using MediatR;

namespace Shared.Application.Common.Mediator;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
