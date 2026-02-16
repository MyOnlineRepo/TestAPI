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

        var normalizedEmail = NormalizeEmail(command.Email);

        EnsureDisplayNameIsValid(command.DisplayName);
        await EnsureEmailIsUniqueAsync(normalizedEmail, cancellationToken);

        var user = User.Create(
            Guid.NewGuid(),
            normalizedEmail,
            command.DisplayName,
            _dateTimeProvider.UtcNow,
            isActive: true);

        await _userRepository.CreateAsync(user, cancellationToken);
    }

    private static void EnsureDisplayNameIsValid(string? displayName)
    {
        ArgumentNullException.ThrowIfNull(displayName);

        if (displayName.Length != 8)
        {
            throw new InvalidOperationException("Display name must be exactly 8 characters long.");
        }

        if (displayName.Any(char.IsWhiteSpace))
        {
            throw new InvalidOperationException("Display name must not contain whitespace.");
        }
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Email must not be empty.");
        }

        return email.Trim();
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
