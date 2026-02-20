using MediatR;
using Shared.Application.AccountSettings.Abstractions;

namespace Shared.Application.AccountSettings.CreateAccountSettings;

public sealed class CreateAccountSettingsCommandHandler : IRequestHandler<CreateAccountSettingsCommand, Guid>
{
	private readonly IAccountSettingsRepository _accountSettingsRepository;

	public CreateAccountSettingsCommandHandler(IAccountSettingsRepository accountSettingsRepository)
	{
		_accountSettingsRepository = accountSettingsRepository;
	}

	public async Task<Guid> Handle(CreateAccountSettingsCommand request, CancellationToken cancellationToken)
	{
		var accountSettingsId = Guid.NewGuid();
		var accountSettings = new Domain.AccountSettings.AccountSettings(accountSettingsId, request.Language);

		await _accountSettingsRepository.AddAsync(accountSettings, cancellationToken);

		return accountSettingsId;
	}
}
