using Shared.Domain.AccountSettings;

namespace Shared.Application.AccountSettings.Abstractions;

public interface IAccountSettingsRepository
{
	Task AddAsync(AccountSettings accountSettings, CancellationToken cancellationToken);
}
