namespace Ids.Shared.Exceptions;

public class EntryNotFoundException : Exception
{
    public EntryNotFoundException() : base("non trouvé")
    {
    }
}