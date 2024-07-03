using System.Data;
using Microsoft.Data.SqlClient;

namespace Ids.Files.Foundations;

public class FileStorage
{
    private readonly string connectionString;

    private const string insertFileQuery = "INSERT dbo.FILES VALUES(@aFileId, @afileName, @aFileData, 0)";

    private const string updateFileQuery =
        "UPDATE dbo.FILES SET FileName = @afileName, Data = @aFileData WHERE FileId = @aFileId";

    private const string deleteFileQuery = "DELETE dbo.FILES WHERE FileId = @aFileId";

    private const string selectFileDataQuery = "SELECT * FROM dbo.FILES WHERE FileId = @aFileId";

    private const string selectFileInfoQuery =
        "SELECT FileId, FileName, ISNULL(len(Data),0) as DataLen, Locked FROM dbo.FILES WHERE FileId = @aFileId";

    private const string fileExistsQuery = "SELECT COUNT(*) FROM dbo.FILES WHERE FileId = @aFileId";

    private const string fileLockedQuery = "SELECT isnull(Locked,0) FROM dbo.FILES WHERE FileId = @aFileId";

    internal FileStorage(string connectionString) =>
        this.connectionString = connectionString;

    public async ValueTask InsertFile(IdsFile file)
    {
        await using var connection = new SqlConnection(connectionString);
        var cmd = new SqlCommand(insertFileQuery, connection);
        cmd.Parameters.AddWithValue("@aFileId", file.FileId.Value);
        cmd.Parameters.AddWithValue("@aFileName", file.FileName);
        cmd.Parameters.AddWithValue("@aFileData", file.Data);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateFile(IdsFile file)
    {
        await using var connection = new SqlConnection(connectionString);
        var cmd = new SqlCommand(updateFileQuery, connection);
        cmd.Parameters.AddWithValue("@aFileId", file.FileId.Value);
        cmd.Parameters.AddWithValue("@aFileName", file.FileName);
        cmd.Parameters.AddWithValue("@aFileData", file.Data);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateFile(FileId fileId, bool used = true)
    {
        await using var connection = new SqlConnection(connectionString);
        var cmd = new SqlCommand(updateFileQuery, connection);
        cmd.Parameters.AddWithValue("@aFileId", fileId.Value);
        cmd.Parameters.AddWithValue("@aLocked", used);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteFile(FileId fileId)
    {
        await using var connection = new SqlConnection(connectionString);
        var cmd = new SqlCommand(deleteFileQuery, connection);
        cmd.Parameters.AddWithValue("@aFileId", fileId.Value);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<IdsFile> SelectFileData(FileId fileId)
    {
        await using var connection = new SqlConnection(connectionString);
        var cmd = new SqlCommand(selectFileDataQuery, connection);
        cmd.Parameters.AddWithValue("@aFileId", fileId.Value);
        await connection.OpenAsync();
        var dt = new DataTable();
        var da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        return dt.Rows.Count != 0 ? new IdsFile(dt.Rows[0], true) : null;
    }

    public IdsFile SelectFileInfo(FileId fileId)
    {
        using var connection = new SqlConnection(connectionString);
        var cmd = new SqlCommand(selectFileInfoQuery, connection);
        cmd.Parameters.AddWithValue("@aFileId", fileId.Value);
        connection.Open();
        var dt = new DataTable();
        var da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        return dt.Rows.Count != 0 ? new IdsFile(dt.Rows[0]) : new IdsFile(fileId);
    }

    public bool FileExists(FileId? fileId)
    {
        using var connection = new SqlConnection(connectionString);
        var cmd = new SqlCommand(fileExistsQuery, connection);
        cmd.Parameters.AddWithValue("@aFileId", fileId?.Value ?? Guid.Empty);
        connection.Open();
        int count = (int)cmd.ExecuteScalar();
        return count > 0;
    }
}