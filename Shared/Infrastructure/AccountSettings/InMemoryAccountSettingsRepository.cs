using Shared.Application.AccountSettings.Abstractions;
using Shared.Domain.AccountSettings;

namespace Shared.Infrastructure.AccountSettings;

public sealed class InMemoryAccountSettingsRepository : IAccountSettingsRepository
{
	private readonly List<Domain.AccountSettings.AccountSettings> _accountSettingsEntries = [];

	public Task AddAsync(Domain.AccountSettings.AccountSettings accountSettings, CancellationToken cancellationToken)
	{
		_accountSettingsEntries.Add(accountSettings);
		return Task.CompletedTask;
	}
}
