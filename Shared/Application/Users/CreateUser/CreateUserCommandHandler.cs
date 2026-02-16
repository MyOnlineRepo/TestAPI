using Shared.Application.Users.Contracts;
using Shared.Domain.Users;

namespace Shared.Application.Users.CreateUser;

public sealed class CreateUserCommandHandler(IUserRepository userRepository)
{
    public async Task<CreateUserResult> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        ValidateCommand(command);

        var utcNow = DateTime.UtcNow;
        var user = User.Create(
            Guid.NewGuid(),
            command.Email.Trim(),
            command.DisplayName.Trim(),
            utcNow);

        await userRepository.CreateAsync(user, cancellationToken);

        return new CreateUserResult(user.Id);
    }

    private static void ValidateCommand(CreateUserCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            throw new ArgumentException("Email must not be empty.", nameof(command));
        }

        if (string.IsNullOrWhiteSpace(command.DisplayName))
        {
            throw new ArgumentException("Display name must not be empty.", nameof(command));
        }
    }
}
