namespace Ids.Shared.Exceptions;

public class InvalidDateException : Exception
{
    public InvalidDateException(string paramName, string paramValue) : base($"la date {paramValue} est invalide ")
    {
    }
}