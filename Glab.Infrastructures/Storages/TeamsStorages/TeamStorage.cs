using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.Extensions.Configuration;

namespace GLAB.Infra.Storages.TeamsStorages;

public class TeamStorage : ITeamStorage
{
    private readonly string connectionString;
    private const string selectTeamsQuery = "select * from dbo.Teams";
    private const string selectTeamByIdQuery = "select * from dbo.Teams where TeamId = @aTeamId";
    private const string selectTeamByNameQuery = "select * from dbo.Teams where TeamName = @aTeamName";
    private const string insertTeamQuery = "dbo.CreateNewTeam";
    private const string updateTeamQuery = "UPDATE dbo.Teams SET TeamName = @aTeamName, LaboratoryId = @aLaboratoryId, Status = @Status WHERE TeamId = @TeamId";
    private const string existIdQuery = "select * from dbo.Teams where TeamId = @aTeamId";
    private const string existNameQuery = "select * from dbo.Teams where TeamName = @aTeamName";
    private const string selectTeamsByLaboratoryQuery = "dbo.GetTeamsByLaboratory";
    private const string updateStatusQuery = "UPDATE dbo.Teams SET Status = @Status WHERE TeamId = @TeamId";
    private const string selectMembersQuery = "SELECT * FROM Members WHERE TeamId = @TeamId";

    public TeamStorage(IConfiguration configuration) =>
        connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane");

    public async Task<bool> IsTeamInLaboratory(string teamId, string laboratoryId)
    {
        Team? team = await SelectTeamById(teamId);
        return team != null && team.LaboratoryId == laboratoryId;
    }

    public async Task<List<Member>> GetTeamMembers(string teamId)
    {
        await using var connection = new SqlConnection(connectionString);

        var cmd = new SqlCommand(selectMembersQuery, connection);
        cmd.Parameters.AddWithValue("@TeamId", teamId);

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        await connection.OpenAsync();

        da.Fill(dt);

        return (from DataRow row in dt.Rows select getMemberFromDataRow(row)).ToList();
    }

    private static Member getMemberFromDataRow(DataRow row)
    {
        return new Member
        {
            MemberId = row["MemberId"].ToString(),
            FirstName = row["FirstName"].ToString(),
            LastName = row["LastName"].ToString(),
            Email = row["Email"].ToString(),
            NIC = row["NIC"].ToString(),
            PhoneNumber = row["PhoneNumber"].ToString(),
            Image = row["Image"] == DBNull.Value ? default : row["Image"] as byte[],
            Status = (MemberStatus)(int)row["Status"],
            TeamId = row["TeamId"].ToString(),
            GradeId = row["GradeId"].ToString(),
            
        };
    }

    public async Task<List<Team>> SelectTeams()
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectTeamsQuery, connection);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return (from DataRow row in ds.Rows select getTeamFromDataRow(row)).ToList();
    }

    public async Task<Team?> SelectTeamById(string teamId)
    {
        await using var connection = new SqlConnection(connectionString);

        SqlCommand cmd = new(selectTeamByIdQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamId", teamId);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count == 0 ? null : getTeamFromDataRow(ds.Rows[0]);
    }

    public async Task<Team?> SelectTeamByName(string TeamName)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectTeamByNameQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamName", TeamName);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count == 0 ? null : getTeamFromDataRow(ds.Rows[0]);
    }

    public async Task InsertTeam(Team team)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(insertTeamQuery, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@TeamId", team.TeamId);
        cmd.Parameters.AddWithValue("@Status",0);
        cmd.Parameters.AddWithValue("@LaboratoryId", team.LaboratoryId);
        cmd.Parameters.AddWithValue("@TeamName", team.TeamName);
        cmd.Parameters.AddWithValue("@TeamAcronyme", team.TeamAcronyme);
        cmd.Parameters.AddWithValue("@TeamLeaderId", team.TeamLeaderId);
        

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateTeam(Team team)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(updateTeamQuery, connection);

        cmd.Parameters.AddWithValue("@aTeamName", team.TeamName);
        cmd.Parameters.AddWithValue("@aStatus", team.Status);
        cmd.Parameters.AddWithValue("@aLaboratoryId", team.LaboratoryId);
        cmd.Parameters.AddWithValue("@aTeamId", team.TeamId);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public static Team getTeamFromDataRow(DataRow row)
    {
        return new()
        {
            TeamName = row["TeamName"] == DBNull.Value ? default : (string)row["TeamName"],
            TeamId = row["TeamId"] == DBNull.Value ? default : (string)row["TeamId"],
            LaboratoryId = row["LaboratoryId"] == DBNull.Value ? default : (string)row["LaboratoryId"],
            Status = (TeamStatus)((int)row["Status"]),
            TeamLeaderId = row["TeamLeaderId"].ToString(),
            TeamAcronyme = row["TeamAcronyme"].ToString()
        };
    }

    public async Task<bool> ExistId(string TeamId)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(existIdQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamId", TeamId);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count != 0;
    }

    public async Task<bool> ExistName(string TeamName)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(existNameQuery, connection);
        cmd.Parameters.AddWithValue("@aTeamName", TeamName);

        DataTable ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);

        return ds.Rows.Count != 0;
    }

    public async Task<List<Team?>> SelectTeamsByLaboratoryId(string LaboratoryId)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectTeamsByLaboratoryQuery, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@labId", LaboratoryId);

        DataSet ds = new();
        SqlDataAdapter da = new(cmd);

        connection.Open();
        da.Fill(ds);
        List<Team> Labteams = new();
        List<Member> Labmembers = new();
        
        if (ds.Tables.Count > 0)
        {
            var teamsTable = ds.Tables[0];
            var membersTable = ds.Tables[1];
            
            foreach (DataRow teamRow in teamsTable.Rows)
            {
                Labteams.Add(getTeamFromDataRow(teamRow));
            }
            
            foreach (DataRow memberRow in membersTable.Rows)
            {
                var member = getMemberFromDataRow(memberRow);
                Labmembers.Add(member);
            }

            foreach (var labTeam in Labteams)
            {
                labTeam.Members.AddRange(
                    Labmembers.FindAll(member =>member.TeamId.Equals(labTeam.TeamId)));
            }

        }

        return Labteams;
    }

    public async Task UpdateTeamStatus(string TeamId, TeamStatus status)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(updateStatusQuery, connection);

        cmd.Parameters.AddWithValue("@Status", (int)status);
        cmd.Parameters.AddWithValue("@TeamId", TeamId);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }


    public async Task<DataSet> GetTeamDataByIdAsync(string teamId)
    {
        string storedProcedureName = "GetTeamById";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TeamId", teamId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();

                await connection.OpenAsync();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
    }


    private List<(string MemberId, string RoleName)> TeamMembersAndRoles { get; set; } = new List<(string MemberId, string RoleName)>();

    public async Task<List<(string MemberId, string RoleName)>> GetTeamMembersAndRolesAsync(string teamId)
    {
        List<(string MemberId, string RoleName)> result = new List<(string MemberId, string RoleName)>();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetTeamMembersAndRoles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TeamId", teamId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            result.Add((reader["MemberId"].ToString(), reader["RoleName"].ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving team members and roles: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> deleteTeam(Team team)
    {
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand DeleteTeamCommand = new SqlCommand("delete from dbo.teams where TeamId=@aTeamId", connection);
            DeleteTeamCommand.Parameters.AddWithValue("@aTeamId", team.TeamId);

            await connection.OpenAsync();

           return await DeleteTeamCommand.ExecuteNonQueryAsync()>0;

        }
    }
}

/*DatabaseService databaseService = new DatabaseService("YourConnectionString");
DataSet dataSet = await databaseService.GetTeamDataByIdAsync("YourTeamId");

// Afficher les données de la première table (VTEAM)
Console.WriteLine("Table VTEAM:");
foreach (DataRow row in dataSet.Tables[0].Rows)
{
foreach (DataColumn col in dataSet.Tables[0].Columns)
{
    Console.Write(col.ColumnName + ": " + row[col] + "\t");
}
Console.WriteLine();
}

// Afficher les données de la deuxième table (VMEMBER)
Console.WriteLine("\nTable VMEMBER:");
foreach (DataRow row in dataSet.Tables[1].Rows)
{
foreach (DataColumn col in dataSet.Tables[1].Columns)
{
    Console.Write(col.ColumnName + ": " + row[col] + "\t");
}
Console.WriteLine();
}

// Afficher les données de la troisième table (VLABORATORY)
Console.WriteLine("\nTable VLABORATORY:");
foreach (DataRow row in dataSet.Tables[2].Rows)
{
foreach (DataColumn col in dataSet.Tables[2].Columns)
{
    Console.Write(col.ColumnName + ": " + row[col] + "\t");
}
Console.WriteLine();
}



*/
