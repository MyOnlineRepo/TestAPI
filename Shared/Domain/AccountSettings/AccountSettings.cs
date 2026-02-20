namespace Shared.Domain.AccountSettings;

public class AccountSettings
{
	public Guid Id { get; }
	public SupportedLanguage Language { get; private set; }

	public AccountSettings(Guid id, SupportedLanguage language)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("Id darf nicht leer sein.", nameof(id));
		}

		Id = id;
		Language = language;
	}
}
