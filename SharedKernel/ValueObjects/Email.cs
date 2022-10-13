using System.Collections.Generic;

namespace SharedKernel.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(Maybe<string> emailOrNothing)
    {
        return emailOrNothing.ToResult("Email should not be empty")
            .OnSuccess(email => email.Trim().ToLower())
            .Ensure(email => email != string.Empty, "Email should not be empty.")
            .Ensure(email => email.Length <= 500, "Email is too long.")
            .Ensure(email => IsValidEmail(email), "Email is invalid")
            .Map(email => new Email(email));
    }

    private static bool IsValidEmail(string email)
    {
        if (email.EndsWith(".")) return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToUpper();
    }

    public static explicit operator Email(string email) => Create(email).Value;
    public static implicit operator string(Email email) => email.Value;
}
