using System.Collections.Concurrent;
using Shared.Application.AccountSettings.Abstractions;

namespace Shared.Infrastructure.AccountSettings;

public sealed class InMemoryAccountSettingsRepository : IAccountSettingsRepository
{
	private readonly ConcurrentDictionary<Guid, Domain.AccountSettings.AccountSettings> _accountSettingsEntries = new();

	public Task AddAsync(Domain.AccountSettings.AccountSettings accountSettings, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(accountSettings);
		cancellationToken.ThrowIfCancellationRequested();

		_accountSettingsEntries[accountSettings.Id] = accountSettings;
		return Task.CompletedTask;
	}
}
