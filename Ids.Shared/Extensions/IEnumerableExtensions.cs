namespace Ids.Shared.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> data, Predicate<T> match)
    {
        List<T> list = data.ToList();
        list.RemoveAll(match);
        return list;
    }
}