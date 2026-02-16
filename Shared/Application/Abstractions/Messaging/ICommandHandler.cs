namespace Shared.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand>
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}
