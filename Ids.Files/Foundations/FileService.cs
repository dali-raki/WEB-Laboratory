using Ids.Files.Exceptions;

namespace Ids.Files.Foundations;

public partial class FileService(string connectionStringName) : IFileService
{
    private readonly FileStorage fileStorage = new(connectionStringName);

    public async ValueTask SaveFile(FileId fileId, string fileName, byte[] data)
    {
        var fileToSave = new IdsFile(fileId, fileName, data);
        await SaveFile(fileToSave);
    }

    public async ValueTask SaveFile(IdsFile file)
    {
        try
        {
            ValidateFile(file);
            ValidateFileNotExists(file.FileId);
            await fileStorage.InsertFile(file);
        }
        catch (InvalidFileException exception)
        {
            throw new FileServiceException("Fichier invalide", exception);
        }
        catch (FileAlreadyExists exception)
        {
            throw new FileServiceException("Un fichier avec le même Id existe déja", exception);
        }
        catch (Exception exception)
        {
            throw new FileServiceException("Echec de la sauvegarde du fichier", exception);
        }
    }

    public bool FileExists(FileId? fileId) => fileStorage.FileExists(fileId);

    public async ValueTask UpdateFile(FileId fileId, string fileName, byte[] data)
    {
        IdsFile file = new(fileId, fileName, data);
        await UpdateFile(file);
    }

    public async ValueTask UpdateFile(IdsFile file)
    {
        try
        {
            ValidateFile(file);

            await fileStorage.UpdateFile(file);
        }
        catch (InvalidFileException exception)
        {
            throw new FileServiceException("Fichier invalide", exception);
        }
        catch (FileNotFoundException exception)
        {
            throw new FileServiceException("Fichier inexistant", exception);
        }
        catch (Exception exception)
        {
            throw new FileServiceException("Echec de la modification du fichier", exception);
        }
    }

    public async ValueTask UpdateFile(FileId fileId, bool used = true)
    {
        try
        {
            ValidateFileExists(fileId);
            await fileStorage.UpdateFile(fileId, used);
        }
        catch (InvalidFileException exception)
        {
            throw new FileServiceException("Fichier invalide", exception);
        }
        catch (FileNotFoundException exception)
        {
            throw new FileServiceException("Fichier inexistant", exception);
        }
        catch (Exception exception)
        {
            throw new FileServiceException("Echec de la modification du fichier", exception);
        }
    }

    public async ValueTask DeleteFile(FileId fileId)
    {
        try
        {
            await fileStorage.DeleteFile(fileId);
        }
        catch (Exception exception)
        {
            throw new FileServiceException("Echec de la suppression du fichier", exception);
        }
    }

    public IdsFile GetInfo(FileId fileId)
    {
        try
        {
            IdsFile file = fileStorage.SelectFileInfo(fileId);

            return file;
        }
        catch (FileNotFoundException exception)
        {
            throw new FileServiceException("Fichier inexistant", exception);
        }
        catch (InvalidFileException exception)
        {
            throw new FileServiceException("Fichier invalide", exception);
        }
        catch (Exception exception)
        {
            throw new FileServiceException("Echec du chargement du fichier", exception);
        }
    }

    public async ValueTask<IdsFile> LoadData(FileId fileId)
    {
        try
        {
            ValidateFileExists(fileId);
            return await fileStorage.SelectFileData(fileId);
        }
        catch (FileNotFoundException exception)
        {
            throw new FileServiceException("Fichier inexistant", exception);
        }
        catch (InvalidFileException exception)
        {
            throw new FileServiceException("Fichier invalide", exception);
        }
        catch (Exception exception)
        {
            throw new FileServiceException("Echec du chargement du fichier", exception);
        }
    }
}