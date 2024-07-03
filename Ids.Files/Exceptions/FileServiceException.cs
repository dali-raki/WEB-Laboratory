namespace Ids.Files.Exceptions;

public class FileServiceException : Exception
{
    public FileServiceException(string message, Exception innerException) : base(message, innerException)
    { }
}