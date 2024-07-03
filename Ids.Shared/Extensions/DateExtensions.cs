namespace Ids.Shared.Extensions;

public static class DateExtensions
{
    public static string GetYear(this DateTime? date) =>
        date != null ? ((DateTime)date).Year.ToString() : string.Empty;

    public static string GetYear(this DateTime date) =>
        date.Year.ToString();

    public static string ToShortDateString(this DateTime? dateTime) =>
        dateTime == null ? string.Empty : ((DateTime)dateTime).ToShortDateString();
}