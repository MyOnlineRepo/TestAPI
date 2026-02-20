using Shared.Application.Common.Mediator;
using Shared.Domain.AccountSettings;

namespace Shared.Application.AccountSettings.CreateAccountSettings;

public sealed record CreateAccountSettingsCommand(SupportedLanguage Language) : ICommand<Guid>;
