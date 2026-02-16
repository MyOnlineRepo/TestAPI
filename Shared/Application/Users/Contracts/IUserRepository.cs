using Shared.Domain.Users;

namespace Shared.Application.Users.Contracts;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
