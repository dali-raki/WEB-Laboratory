namespace Ids.Files.Exceptions;

public class FileLockedException : Exception
{
    public FileLockedException(string fileId) : base()
    {
    }
}