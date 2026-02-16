using Shared.Application.Abstractions.Messaging;
using Shared.Application.Abstractions.Time;
using Shared.Application.Users.Contracts;
using Shared.Domain.Users;

namespace Shared.Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        await EnsureEmailIsUniqueAsync(command.Email, cancellationToken);

        var user = User.Create(
            Guid.NewGuid(),
            command.Email,
            command.DisplayName,
            _dateTimeProvider.UtcNow,
            isActive: true);

        await _userRepository.CreateAsync(user, cancellationToken);
    }

    private async Task EnsureEmailIsUniqueAsync(string email, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException($"A user with email '{email}' already exists.");
        }
    }
}
