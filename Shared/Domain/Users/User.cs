using System.Net.Mail;

namespace Shared.Domain.Users;

public sealed class User
{
    public Guid Id { get; }
    public string Email { get; private set; }
    public string DisplayName { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }
    public bool IsActive { get; private set; }

    private User(
        Guid id,
        string email,
        string displayName,
        DateTime createdAtUtc,
        DateTime updatedAtUtc,
        bool isActive)
    {
        Id = EnsureValidId(id);
        Email = EnsureValidEmail(email);
        DisplayName = EnsureValidDisplayName(displayName);
        CreatedAtUtc = EnsureUtc(createdAtUtc, nameof(createdAtUtc));
        UpdatedAtUtc = EnsureUtc(updatedAtUtc, nameof(updatedAtUtc));
        EnsureUpdatedAfterOrEqualCreated(CreatedAtUtc, UpdatedAtUtc);
        IsActive = isActive;
    }

    public static User Create(
        Guid id,
        string email,
        string displayName,
        DateTime createdAtUtc,
        bool isActive = true)
    {
        var normalizedCreatedAtUtc = EnsureUtc(createdAtUtc, nameof(createdAtUtc));

        return new User(
            id,
            email,
            displayName,
            normalizedCreatedAtUtc,
            normalizedCreatedAtUtc,
            isActive);
    }

    public void ChangeEmail(string email, DateTime updatedAtUtc)
    {
        Email = EnsureValidEmail(email);
        SetUpdatedAtUtc(updatedAtUtc);
    }

    public void ChangeDisplayName(string displayName, DateTime updatedAtUtc)
    {
        DisplayName = EnsureValidDisplayName(displayName);
        SetUpdatedAtUtc(updatedAtUtc);
    }

    public void Activate(DateTime updatedAtUtc)
    {
        IsActive = true;
        SetUpdatedAtUtc(updatedAtUtc);
    }

    public void Deactivate(DateTime updatedAtUtc)
    {
        IsActive = false;
        SetUpdatedAtUtc(updatedAtUtc);
    }

    private void SetUpdatedAtUtc(DateTime updatedAtUtc)
    {
        var normalizedUpdatedAtUtc = EnsureUtc(updatedAtUtc, nameof(updatedAtUtc));
        EnsureUpdatedAfterOrEqualCreated(CreatedAtUtc, normalizedUpdatedAtUtc);
        UpdatedAtUtc = normalizedUpdatedAtUtc;
    }

    private static Guid EnsureValidId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id must not be empty.", nameof(id));
        }

        return id;
    }

    private static string EnsureValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must not be empty.", nameof(email));
        }

        var normalizedEmail = email.Trim();

        if (!MailAddress.TryCreate(normalizedEmail, out var parsedEmail) ||
            !string.Equals(parsedEmail.Address, normalizedEmail, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Email has an invalid format.", nameof(email));
        }

        return normalizedEmail;
    }

    private static string EnsureValidDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name must not be empty.", nameof(displayName));
        }

        return displayName.Trim();
    }

    private static DateTime EnsureUtc(DateTime value, string parameterName)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("Date time must be in UTC.", parameterName);
        }

        return value;
    }

    private static void EnsureUpdatedAfterOrEqualCreated(DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        if (updatedAtUtc < createdAtUtc)
        {
            throw new ArgumentException("UpdatedAtUtc must be greater than or equal to CreatedAtUtc.", nameof(updatedAtUtc));
        }
    }
}
