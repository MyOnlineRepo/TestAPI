using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.AccountSettings;
using Shared.Application.AccountSettings.CreateAccountSettings;
using Shared.Domain.AccountSettings;

namespace Presentation.Controllers.AccountSettings;

[ApiController]
[Route("account-settings")]
public class AccountSettingsController : ControllerBase
{
	private readonly ISender _sender;

	public AccountSettingsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpPost]
	public async Task<ActionResult<CreateAccountSettingsResponse>> CreateAsync(
		[FromBody] CreateAccountSettingsRequest request,
		CancellationToken cancellationToken)
	{
		if (!TryParseLanguage(request.Language, out var supportedLanguage))
		{
			return BadRequest("Language muss De oder En sein.");
		}

		var accountSettingsId = await _sender.Send(new CreateAccountSettingsCommand(supportedLanguage), cancellationToken);
		return CreatedAtAction(nameof(CreateAsync), new CreateAccountSettingsResponse(accountSettingsId));
	}

	private static bool TryParseLanguage(string languageInput, out SupportedLanguage supportedLanguage)
	{
		supportedLanguage = default;
		if (string.IsNullOrWhiteSpace(languageInput))
		{
			return false;
		}

		return Enum.TryParse(languageInput, true, out supportedLanguage) &&
			Enum.IsDefined(supportedLanguage);
	}
}
