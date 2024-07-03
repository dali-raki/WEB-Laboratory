namespace Ids.Files.Foundations;

public interface IFileService
{
    ValueTask SaveFile(IdsFile file);

    ValueTask SaveFile(FileId fileId, string fileName, byte[] data);

    ValueTask UpdateFile(IdsFile file);

    ValueTask UpdateFile(FileId fileId, string fileName, byte[] data);

    ValueTask UpdateFile(FileId fileId, bool used = true);

    ValueTask DeleteFile(FileId fileId);

    ValueTask<IdsFile> LoadData(FileId fileId);

    IdsFile GetInfo(FileId fileId);

    bool FileExists(FileId? fileId);
}