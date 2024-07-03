namespace Ids.Files.Exceptions;

public class FileAlreadyExists : Exception
{
    public FileAlreadyExists(Guid fileId) : base()
    {
    }
}