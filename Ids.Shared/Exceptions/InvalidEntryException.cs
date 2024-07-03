namespace Ids.Shared.Exceptions;

public class InvalidEntryException : Exception
{
    public InvalidEntryException() :
        base()
    {
    }

    public InvalidEntryException(string paramName, string paramValue) :
        base($"Donnée invalide : {paramName}: {paramValue}")
    {
    }
}