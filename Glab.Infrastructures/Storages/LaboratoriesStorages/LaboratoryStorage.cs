using System.Data;
using System.Data.SqlClient;
using System.IO;
using Glab.Domains.Models.Faculties;
using Glab.Domains.Models.Roles;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using GLAB.Infra.Storages.LaboratoriesStorages;
using GLAB.Infra.Storages.MembersStorages;
using GLAB.Infra.Storages.TeamsStorages;
using Microsoft.Extensions.Configuration;

namespace Glab.Infrastructures.Storages.LaboratoriesStorages;

public class LaboratoryStorage : ILaboratoryStorage
{
    private readonly string connectionString;
    private IMemberStorage memberStorage { get; set; }


    public LaboratoryStorage(IConfiguration configuration ,IMemberStorage memberStorage)
    {
        this.memberStorage = memberStorage;
        connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane") ?? String.Empty;
    }

    private const string updateCommandText = "UPDATE dbo.LABORATORY SET Status = -1 WHERE Id = @aLaboratoryId";

    public async Task UpdateLaboratoryStatus(string id)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(updateCommandText, connection);

        cmd.Parameters.AddWithValue("@aLaboratoryId", id);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Laboratory>> SelectLaboratories()
    {
        var laboratories = new List<Laboratory>();

        try
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * ,  Faculty.Name AS FacultyName,Faculty.Acronyme AS FacultyAcronyme FROM VLABORATORY INNER JOIN Faculty ON VLABORATORY.FacultyId = Faculty.FacultyId;", connection);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);


            connection.Open();

             connection.Open();

            da.Fill(dt);

            Console.WriteLine($"Rows retrieved: {dt.Rows.Count}");

            foreach (DataRow row in dt.Rows)
            {
                var laboratory = getLaboratoireFromDataRow(row);
                if (laboratory != null)
                {
                    laboratories.Add(laboratory);
                    Console.WriteLine($"Added laboratory: {laboratory.Name}");
                }
                else
                {
                    Console.WriteLine("Failed to parse laboratory from row.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return laboratories;
    }
    private const string updateLaboratoryCommand = @"
        UPDATE dbo.LABORATORY
        SET University  = @aUniversity,
            DirectorId = @aDirectorId,
            Adress = @aAdresse,
            Logo = @aLogo,
            PhoneNumber = @aPhoneNumber,
            WebSite = @aWebSite
        WHERE Email = @aEmail";

    public async Task UpdateLaboratory(Laboratory laboratory)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand cmd = new SqlCommand(updateLaboratoryCommand, connection);

        cmd.Parameters.AddWithValue("@aUniversity", laboratory.University);
        cmd.Parameters.AddWithValue("@aAdresse", laboratory.Adresse);
        cmd.Parameters.AddWithValue("@aPhoneNumber", laboratory.PhoneNumber);
        cmd.Parameters.AddWithValue("@aDirectorId", laboratory.DirectorId);
        cmd.Parameters.AddWithValue("@aLogo", laboratory.Logo);
        cmd.Parameters.AddWithValue("@aWebSite", laboratory.WebSite);
        cmd.Parameters.AddWithValue("@aEmail", laboratory.Email);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }


private const string insertLaboratoryCommand =
        "Insert INTO dbo.LABORATORY (LaboratoryId,Name,Email,AgrementNumber,AgrementDate,University,FacultyId,CreationDate,Acronyme,Status) " +
        "VALUES(@aLaboratoryId,@aName,@aEmail,@aAgrementNumber,@aAgrementDate,@aUniversity,@aFacultyId,@aCreationDate,@aAcronyme,@aStatus)";

    public async Task InsertLaboratory(Laboratory laboratory)
    {
        await using SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(insertLaboratoryCommand, connection);

        cmd.Parameters.AddWithValue("@aLaboratoryId", laboratory.LaboratoryId);
        cmd.Parameters.AddWithValue("@aName", laboratory.Name);
        cmd.Parameters.AddWithValue("@aEmail", laboratory.Email);
        cmd.Parameters.AddWithValue("@aAgrementNumber", laboratory.NumAgrement);
        cmd.Parameters.AddWithValue("@aAgrementDate", new DateTime( laboratory.AgrementDate,TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("@aUniversity", "Biskra");
        cmd.Parameters.AddWithValue("@aFacultyId", laboratory.FacultyId);
        cmd.Parameters.AddWithValue("@aCreationDate", DateTime.Today);
        cmd.Parameters.AddWithValue("@aAcronyme", laboratory.Acronyme);
        cmd.Parameters.AddWithValue("@aStatus", 1);
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }


    private static Laboratory getLaboratoireFromDataRow(DataRow row)
    {
        try
        {
            return new Laboratory
            {
                LaboratoryId = row["LaboratoryId"] != DBNull.Value ? row["LaboratoryId"].ToString() : string.Empty,
                DirectorId = row["DirectorId"] != DBNull.Value ? row["DirectorId"].ToString() : string.Empty,
                Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : string.Empty,
                Adresse = row["Adress"] != DBNull.Value ? row["Adress"].ToString() : string.Empty,
                University = row["University"] != DBNull.Value ? row["University"].ToString() : string.Empty,
                PhoneNumber = row["PhoneNumber"] != DBNull.Value ? row["PhoneNumber"].ToString() : string.Empty,
                Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : string.Empty,
                NumAgrement = row["AgrementNumber"] != DBNull.Value ? row["AgrementNumber"].ToString() : string.Empty,
                CreationDate = row["CreationDate"] != DBNull.Value ? Convert.ToDateTime(row["CreationDate"]) : DateTime.MinValue,
                AgrementDate = row["AgrementDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["AgrementDate"])) : DateOnly.MinValue,
                WebSite = row["WebSite"] != DBNull.Value ? row["WebSite"].ToString() : string.Empty,
                Status = row["Status"] != DBNull.Value ? (LaboratoryStatus)Enum.Parse(typeof(LaboratoryStatus), row["Status"].ToString()) : LaboratoryStatus.Deleted,
                Faculte = new Faculty
                {
                    FacultyId = row["FacultyId"] != DBNull.Value ? row["FacultyId"].ToString() : string.Empty,
                }
            };
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            Console.WriteLine($"Error parsing DataRow to Laboratory: {ex.Message}");
            return null;
        }
    }
    private const string selectLaboratoryCountByAcronymeQuery = "SELECT COUNT(*) FROM VLABORATORY WHERE Acronyme = @aAcronyme";

    public async Task<bool> LaboratoryExistsByAcronyme(string acronyme)
    {
        await using var connection = new SqlConnection(connectionString);
        connection.Open();

        SqlCommand command = new SqlCommand(selectLaboratoryCountByAcronymeQuery, connection);
        command.Parameters.AddWithValue("@aAcronyme", acronyme);

        int count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    private const string selectLaboratoryCountByIdQuery = "SELECT COUNT(*) FROM LABORATORY WHERE LaboratoryId = @Id";

    public async Task<bool> LaboratoryExistsById(string id)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand command = new SqlCommand(selectLaboratoryCountByIdQuery, connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();

        int count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

    private const string selectLaboratoryStatusByIdQuery = "SELECT Status FROM VLABORATORY WHERE LaboratoryId = @aId";

    public async Task<LaboratoryStatus> GetLaboratoryStatus(string id)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectLaboratoryStatusByIdQuery, connection);
        command.Parameters.AddWithValue("@aId", id);
        connection.Open();

        return (LaboratoryStatus)(await command.ExecuteScalarAsync() ?? LaboratoryStatus.Deleted);
    }

    private const string selectLaboratoryByIdQuery = "GetLaboratoryById";

    public async Task<Laboratory> SelectLaboratoryById(string id)
    {
        using var connection = new SqlConnection(connectionString);
        var command = new SqlCommand(selectLaboratoryByIdQuery, connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@aLaboratoryId", id);

        DataSet ds = new DataSet();

        await connection.OpenAsync();
        SqlDataAdapter da = new SqlDataAdapter(command);

        da.Fill(ds);

        DataTable laboratoryTable = ds.Tables[0];
        DataTable teamTable = ds.Tables[1];
        DataTable memberTable = ds.Tables[2];

        Laboratory laboratory = getLaboratoireFromDataRow(laboratoryTable.Rows[0]);

        foreach (DataRow row in teamTable.Rows)
        {
            Team team = TeamStorage.getTeamFromDataRow(teamTable.Rows[1]);


            foreach (DataRow memberRow in memberTable.Rows)
            {
                Member member = MemberStorage.getMemberFromDataRow(memberTable.Rows[2]);
                List<Role> roles = await memberStorage.SelectRolesOfMember(member.MemberId);
                foreach (Role role in roles)
                {
                    member.Roles.Add(role);

                }
                team.Members.Add(member);
            }
            laboratory.Teams.Add(team);

        }

        return laboratory;
    }


    private const string selectLaboratoryByNameQuery = "SELECT COUNT(*), Faculty.Name AS FacultyName,Faculty.Acronyme AS FacultyAcronyme FROM VLABORATORY INNER JOIN Faculty ON VLABORATORY.FacultyId = Faculty.FacultyId FROM VLABORATORY WHERE name = @name";

    public async Task<Laboratory?> SelectLaboratoryByName(string name)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectLaboratoryByNameQuery, connection);

        command.Parameters.AddWithValue("@aname", name);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();

        Console.Write(dt.Rows.Count);
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        else
        {
            return getLaboratoireFromDataRow(dt.Rows[0]);
        }
    }
    public async Task<bool> LaboratoryExistsByName(string name)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM VLABORATORY WHERE Name = @aName";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@aName", name);

                int count = (int)command.ExecuteScalar();

                return count > 0; ;
            }
        }

    }
        private const string selectLaboratoryByEmailQuery = "SELECT *,Faculty.Name AS FacultyName,Faculty.Acronyme AS FacultyAcronyme FROM VLABORATORY INNER JOIN Faculty ON VLABORATORY.FacultyId = Faculty.FacultyId WHERE Email = @aEmail";

    public async Task<Laboratory?> SelectLaboratoryByEmail(string email)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectLaboratoryByEmailQuery, connection);

        command.Parameters.AddWithValue("@aEmail", email);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();
        da.Fill(dt);

        return dt.Rows.Count == 0 ? null : getLaboratoireFromDataRow(dt.Rows[0]);
    }
    private const string selectLaboratoryInformationsQuery = @" 
    SELECT 
        VLABORATORY.LaboratoryId,
        VLABORATORY. Name,
       VLABORATORY.  Email,
       VLABORATORY. AgrementNumber,
       VLABORATORY. CreationDate,
       VLABORATORY. AgrementDate,
       VLABORATORY. University,
      VLABORATORY.  Acronyme,
       VLABORATORY.  Adress,
        VLABORATORY.DirectorId,
        VLABORATORY.PhoneNumber,
        VLABORATORY.WebSite,

Faculty.Name AS FacultyName
from 
VLABORATORY INNER JOIN Faculty ON VLABORATORY.FacultyId = Faculty.FacultyId  WHERE 
        Email = @aEmail";
    public async Task<Laboratory?> SelectLaboratoryInformations(string email)
    {
        Laboratory labo = null;
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectLaboratoryInformationsQuery, connection);

        command.Parameters.AddWithValue("@aEmail", email);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();
        da.Fill(dt);

        Console.Write(dt.Rows.Count);

        DataRow row = dt.Rows[0];
        labo = new Laboratory
        {
            LaboratoryId = row["LaboratoryId"] != DBNull.Value ? row["LaboratoryId"].ToString() : string.Empty,
            Acronyme = row["Acronyme"] != DBNull.Value ? row["Acronyme"].ToString() : string.Empty,
            Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : string.Empty,
            University = row["University"] != DBNull.Value ? row["University"].ToString() : string.Empty,
            Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : string.Empty,
            NumAgrement = row["AgrementNumber"] != DBNull.Value ? row["AgrementNumber"].ToString() : string.Empty,
            AgrementDate = row["AgrementDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(row["AgrementDate"])) : DateOnly.MinValue,
            Adresse = row["Adress"] != DBNull.Value ? row["Adress"].ToString() : string.Empty,
            PhoneNumber = row["PhoneNumber"] != DBNull.Value ? row["PhoneNumber"].ToString() : string.Empty,
            WebSite = row["WebSite"] != DBNull.Value ? row["WebSite"].ToString() : string.Empty,
            DirectorId = row["DirectorId"] != DBNull.Value ? row["DirectorId"].ToString() : string.Empty,
            Faculte = new Faculty
            {
                Name = row["FacultyName"] != DBNull.Value ? row["FacultyName"].ToString() : string.Empty,

            }
        };

        return labo;
    }


    private const string selectLaboratoryByUserQuery = "dbo.GetLaboratoryByUser";
    public async Task<Laboratory> SelectLaboratoryByUser(string userID)
    {
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand selectLaboratoryByUserCommand = new SqlCommand(selectLaboratoryByUserQuery, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            selectLaboratoryByUserCommand.Parameters.AddWithValue("@UserId",userID);
            
            SqlDataAdapter adapter = new(selectLaboratoryByUserCommand);
            DataSet set = new();
            adapter.Fill(set);
            
            await connection.OpenAsync();
            var targetLabTable = set.Tables[0];
            var targetLabRow = targetLabTable.Rows[0];

            return getLaboratoireFromDataRow(targetLabRow);
        }
    }

    


    private const string LaboratoryExistsByAgrementNumQuery = "SELECT COUNT(*) FROM LABORATORY WHERE AgrementNumber = @aAgrementNumber";

    public async Task<bool> LaboratoryExistsByAgrementNum(string AgrementNumber)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand command = new SqlCommand(LaboratoryExistsByAgrementNumQuery, connection);
        command.Parameters.AddWithValue("@aAgrementNumber", AgrementNumber);
        connection.Open();

        int count = (int)(await command.ExecuteScalarAsync() ?? 0);

        return count > 0;
    }

}
