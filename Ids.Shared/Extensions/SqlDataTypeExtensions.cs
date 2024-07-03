namespace Ids.Shared.Extensions;

public static class DataTypeExtensions
{
    public static string AsString(this object obj) => (string)obj;

    public static Guid AsGuid(this object obj)
    {
        try
        {
            return Guid.Parse(obj.ToString());
        }
        catch (Exception e)
        {
            return new Guid();
        }
    }

    public static bool AsBool(this object obj) => (bool)obj;

    public static int AsInt(this object obj) => (int)obj;

    public static DateTime AsDateTime(this object obj) => (DateTime)obj;

    public static decimal AsDecimal(this object obj) => (decimal)obj;
}