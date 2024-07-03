using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Ids.Shared.Exceptions;

namespace Ids.Shared.Validations;

public static class Validators
{
    public static void ValidateNotNull(object entry)
    {
        if (entry == null)
            throw new NullEntryException(nameof(entry));
    }

    public static void Validate(string entry)
    {
        if (string.IsNullOrWhiteSpace(entry))
            throw new InvalidEntryException(nameof(entry), string.Empty);
    }

    public static void Validate(byte[] entry)
    {
        if (entry.Length == 0)
            throw new InvalidEntryException(nameof(entry), string.Empty);
    }

    public static void Validate(int entry)
    {
        if (entry == 0)
            throw new InvalidEntryException(nameof(entry), "0");
    }

    public static void Validate(DateTime entry)
    {
        if (entry == default)
            throw new InvalidEntryException(nameof(entry), entry.ToString(CultureInfo.InvariantCulture));
    }

    public static void ValidateYear(string year)
    {
        Regex regex = new("[0-9]{4}");
        if (regex.IsMatch(year) == false)
            throw new InvalidEntryException(nameof(year), year);
    }

    public static void ValidatePIC(string pic)
    {
        Regex regex = new("[0-9]{9}");
        if (regex.IsMatch(pic) == false)
            throw new InvalidEntryException(nameof(pic), pic);
    }

    public static void ValidateAnneeUniversitaire(string AnneeUniversitaire)
    {
        if (Regex.IsMatch(AnneeUniversitaire, "[0-9]{4}/[0-9]{4}") == false)
            throw new InvalidAnneeEnseignementException(nameof(AnneeUniversitaire));
    }

    public static void ValidateEmail(string email)
    {
        try
        {
            var result = new MailAddress(email);
        }
        catch
        {
            throw new InvalidEntryException(nameof(email), email);
        }
    }
}