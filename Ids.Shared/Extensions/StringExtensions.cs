namespace Ids.Shared.Extensions;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => null,
            "" => input,
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };

    public static string ToBase64Image(this byte[] bytes) => "data:image/png;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);
}