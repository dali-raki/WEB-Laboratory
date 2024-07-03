using System.Data;
using Ids.Shared.Extensions;

namespace Ids.Files;

public class IdsFile
{
    public FileId FileId { get; set; }
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public bool HasData { get; set; }
    public bool IsValid => !string.IsNullOrWhiteSpace(FileId?.Value.ToString()) && !string.IsNullOrWhiteSpace(FileName) && HasData;

    public IdsFile()
    {
        Data = new byte[] { };
        HasData = false;
    }

    public IdsFile(FileId fileId) : this() => FileId = fileId;

    public IdsFile(FileId fileId, string fileName, byte[] data)
    {
        FileId = fileId;
        FileName = fileName;
        Data = data;
        HasData = data is null ? false : data.Length > 0;
    }

    public IdsFile(IdsFile file)
    {
        FileId = file.FileId;
        FileName = file.FileName;
        Data = file.Data;
        HasData = file.Data is null ? false : file.Data.Length > 0;
    }

    public IdsFile(DataRow row, bool includeData = false)
    {
        FileId = new FileId(row["FileId"].AsGuid());
        FileName = (string)row["FileName"];
        HasData = ((byte[])row["Data"]).Length != 0;
        if (includeData) Data = (byte[])row["Data"];
    }

    public IdsFile(DataRow row)
    {
        FileId = new FileId(row["FileId"].AsGuid());
        FileName = (string)row["FileName"];
        HasData = Convert.ToInt32(row["DataLen"]) != 0;
    }
}