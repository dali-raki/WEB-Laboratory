using System.ComponentModel;
using System.Reflection;

namespace Ids.Shared.Extensions;

public static class EnumsExtensions
{
    public static string Description(this Enum value)
    {
        FieldInfo? fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }

    public static object Value(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DefaultValueAttribute[] attributes = (DefaultValueAttribute[])fi.GetCustomAttributes(typeof(DefaultValueAttribute), false);

        return attributes.Length > 0 ? attributes[0].Value : null;
    }

    public static string Category(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        CategoryAttribute[] attributes = (CategoryAttribute[])fi.GetCustomAttributes(typeof(CategoryAttribute), false);

        return attributes.Length > 0 ? attributes[0].Category : value.ToString();
    }

    public static bool IsBindable(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        BindableAttribute attributes = (BindableAttribute)fi.GetCustomAttributes(typeof(BindableAttribute), false).FirstOrDefault();

        return attributes != null && attributes.Bindable;
    }

    public static List<KeyValuePair<TEnum, string>> GetList<TEnum>()
        where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();
        return ((TEnum[])Enum.GetValues(typeof(TEnum)))
            .ToDictionary(k => k, v => ((Enum)(object)v).ToString())
            .ToList();
    }
}