using Ids.Files.Exceptions;

namespace Ids.Files.Foundations;

public partial class FileService
{
    private void ValidateFileNotExists(FileId fileId)
    {
        if (FileExists(fileId))
            throw new FileAlreadyExists(fileId.Value);
    }

    private void ValidateFileExists(FileId fileId)
    {
        if (!FileExists(fileId))
            throw new FileNotFoundException();
    }

    private static void ValidateFile(IdsFile file)
    {
        if (string.IsNullOrWhiteSpace(file.FileId.Value.ToString()) ||
            string.IsNullOrWhiteSpace(file.FileName) ||
            file.Data is null || file.Data.Length == 0)
        {
            throw new InvalidFileException();
        }
    }
}