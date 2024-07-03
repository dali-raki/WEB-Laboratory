namespace Ids.Shared.Exceptions;

public class CriticalException : Exception
{
    public CriticalException(string message, Exception innereException) : base(message, innereException)
    {
    }
}