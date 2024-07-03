namespace Ids.Shared.Exceptions;

public class NullEntryException : Exception
{
    public NullEntryException(string objectName) :
        base($"L'objet {objectName} est vide")
    {
    }
}