using System.Data;
using System.Data.SqlClient;
using Glab.Domains.Models.Grades;
using Glab.Domains.Models.Roles;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Models.Members;
using Microsoft.Extensions.Configuration;

namespace GLAB.Infra.Storages.MembersStorages;

public class MemberStorage : IMemberStorage
{
    private readonly string connectionString;

    public MemberStorage(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Redouane");
    }

    public string updateMemberSQLcommand =
        "update dbo.members SET FirstName = @aFirstName , LastName = @aLastName,NIC = @aNIC," +
        "PhoneNumber = @aPhoneNumber," +
        "GradeId = @aGradeId," +
        "Status = @aStatus,TeamId = @aTeamId ,Email = @aEmail " +
        "WHERE MemberId = @aMemberId";

    public string insertMemberSQLcommand =
        "INSERT INTO dbo.Members(MemberId, FirstName, LastName, Email, NIC, TeamId, PhoneNumber, Image, status) " +
        "VALUES(@aMemberId, @aFirstName, @aLastName, @aEmail, @aNIC, @aTeamId, @aPhoneNumber, @aImage, @aStatus)";



    public async Task UpdateMemberStatus(string id)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        string updateCommandText = @"
        UPDATE dbo.Members
        SET Status = -1
        WHERE Id = @aMemberId";
        
        SqlCommand cmd = new SqlCommand(updateCommandText, connection);

        cmd.Parameters.AddWithValue("@aMemberId", id);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task InsertMember(Member member)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        SqlCommand cmd = new SqlCommand(insertMemberSQLcommand, connection);

        cmd.Parameters.AddWithValue("@aMemberId", member.MemberId);
        cmd.Parameters.AddWithValue("@aFirstName", member.FirstName);
        cmd.Parameters.AddWithValue("@aLastName", member.LastName);
        cmd.Parameters.AddWithValue("@aEmail", member.Email);
        cmd.Parameters.AddWithValue("@aNIC", member.NIC);
        cmd.Parameters.AddWithValue("@aPhoneNumber", member.PhoneNumber);
        cmd.Parameters.AddWithValue("@aTeamId", member.TeamId);
        cmd.Parameters.AddWithValue("@aImage", member.Image);
        cmd.Parameters.AddWithValue("@aStatus", 1);

        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }


    public async Task<List<Member>> SelectMembers()
    {
        List<Member> members = new List<Member>();
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new("SELECT * from VMember", connection);

        DataTable dt = new();
        SqlDataAdapter da = new(cmd);

        await connection.OpenAsync();
        da.Fill(dt);

        foreach (DataRow row in dt.Rows)
        {
            Member member = getMemberFromDataRow(row);
            members.Add(member);
        }

        return members;
    }


    // DALI ADD SELECT DIRECTOR

    public async Task<List<Member>> SelectDirector()
    {
        List<Member> members = new List<Member>();
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(" SELECT * FROM VMember as member  left join dbo.MEMBERSROLES roles on roles.RoleId ='2' and roles.MemberId = member.MemberId WHERE GradeId = 1 OR GradeId = 2", connection);

        DataTable dt = new();
        SqlDataAdapter da = new(cmd);

        await connection.OpenAsync();
        da.Fill(dt);

        foreach (DataRow row in dt.Rows)
        {
            Member member = getMemberFromDataRow(row);
            members.Add(member);
        }

        return members;
    }

    // ******************************





    public async Task<bool> UpdateMember(Member member)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(updateMemberSQLcommand, connection);
        cmd.Parameters.AddWithValue("@aFirstName", member.FirstName);
        cmd.Parameters.AddWithValue("@aLastName", member.LastName);
        cmd.Parameters.AddWithValue("@aEmail", member.Email);
        cmd.Parameters.AddWithValue("@aNIC", member.NIC);
        cmd.Parameters.AddWithValue("@aPhoneNumber", member.PhoneNumber);
        cmd.Parameters.AddWithValue("@aGradeId", member.GradeId);
        cmd.Parameters.AddWithValue("@aTeamId", member.TeamId);
        cmd.Parameters.AddWithValue("@aStatus", member.Status);
        cmd.Parameters.AddWithValue("@aMemberId", member.MemberId);
        
     
        await connection.OpenAsync();
        
        return await cmd.ExecuteNonQueryAsync()>0;
    }

    public static Member getMemberFromDataRow(DataRow row)
    {
        
        return new Member
        {
       
            MemberId = row["MemberId"] == DBNull.Value ? default : (string)row["MemberId"],
            FirstName = row["FirstName"] == DBNull.Value ? default : (string)row["FirstName"],
            LastName = row["LastName"] == DBNull.Value ? default : (string)row["LastName"],
            Email = row["Email"] == DBNull.Value ? default : (string)row["Email"],
            NIC = row["NIC"] == DBNull.Value ? default : (string)row["NIC"],
            PhoneNumber = row["PhoneNumber"] == DBNull.Value ? default : (string)row["PhoneNumber"],
            Image = row["Image"] == DBNull.Value ? default : (byte[])row["Image"],
            TeamId = row["TeamId"] == DBNull.Value ? default : (string)row["TeamId"],
            GradeId = row["GradeId"] == DBNull.Value ? default : (string)row["GradeId"]
    };
    }

    public async Task<bool> MemberExistsById(string id)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        string query = "SELECT COUNT(*) FROM VMembers WHERE MemberId = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        int count = (int)await command.ExecuteScalarAsync();

        return count > 0;
    }


    private const string selectMemberByEmailQuery = "SELECT * FROM MEMBERS WHERE email = @aemail";

    public async Task<Member> SelectMemberByEmail(string email)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectMemberByEmailQuery, connection);

        command.Parameters.AddWithValue("@aemail", email);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
         
         connection.Open();
        da.Fill(dt);

        Console.Write(dt.Rows.Count);
        return dt.Rows.Count == 0 ? null : getMemberFromDataRow(dt.Rows[0]);
    }
    private const string selectMemberByNameQuery = "SELECT COUNT(*) FROM VMembers WHERE name = @aname";

    public async Task<Member> SelectMemberByName(string name)
    {
        await using var connection = new SqlConnection(connectionString);

        var command = new SqlCommand(selectMemberByEmailQuery, connection);

        command.Parameters.AddWithValue("@aname", name);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(command);
        await connection.OpenAsync();

        return dt.Rows.Count == 0 ? null : getMemberFromDataRow(dt.Rows[0]);
    }
    public async Task AddMemberRole(string memberId, string roleId)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        string insertCommandText = @"
                INSERT INTO MembersRoles (MemberId, RoleId)
                VALUES (@MemberId, @RoleId)";
        SqlCommand cmd = new SqlCommand(insertCommandText, connection);
        cmd.Parameters.AddWithValue("@MemberId", memberId);
        cmd.Parameters.AddWithValue("@RoleId", roleId);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task RemoveMemberRole(string memberId, string roleId)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        string deleteCommandText = @"
                DELETE FROM MembersRoles
                WHERE MemberId = @MemberId AND RoleId = @RoleId";
        SqlCommand cmd = new SqlCommand(deleteCommandText, connection);
        cmd.Parameters.AddWithValue("@MemberId", memberId);
        cmd.Parameters.AddWithValue("@RoleId", roleId);
        await cmd.ExecuteNonQueryAsync();
    }
    public async Task<List<string>> SelectMemberRoles(string memberId)
    {
        List<string> roles = new List<string>();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        string selectCommandText = @"
        SELECT Roles.RoleName
        FROM MembersRoles
        JOIN Roles ON MembersRoles.RoleId = Roles.RoleId
        WHERE MembersRoles.MemberId = @MemberId";

        SqlDataAdapter da = new SqlDataAdapter(selectCommandText, connection);
        da.SelectCommand.Parameters.AddWithValue("@MemberId", memberId);

        DataTable dt = new DataTable();
        da.Fill(dt);

        foreach (DataRow row in dt.Rows)
        {
            roles.Add(row["RoleName"].ToString());
        }

        return roles;
    }
private const string selectRolesOfMemberQuery = "GetRolesOfMember";

public async Task<List<Role>> SelectRolesOfMember(string memberId)
{
    List<Role> roles = new List<Role>();


        try
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(selectRolesOfMemberQuery, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@aMemberId", memberId);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(command);

            await connection.OpenAsync();
            da.Fill(dt);

            roles = (from DataRow row in dt.Rows select getRoleFromDataRow(row)).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return roles;

      

  
}


private const string selectMembersByLabQuery = "dbo.GetMembersByLab";

public async Task<List<Member>> SelectMembersByLab(string labId)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand selectMembersByLabCommand = new SqlCommand(selectMembersByLabQuery,connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        selectMembersByLabCommand.Parameters.AddWithValue("@labId", labId);

        DataSet dataSet = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(selectMembersByLabCommand);

        await connection.OpenAsync();
        da.Fill(dataSet);
        List<Member> members = new List<Member>();
        
        if (dataSet.Tables.Count > 0)
        {
            foreach (DataRow memberRow in dataSet.Tables[0].Rows)
            {
                members.Add(getMemberFromDataRow(memberRow));
            } 
        }

        return members;
    }
}

private static Role? getRoleFromDataRow(DataRow row)
    {
        return new()
        {
            RoleId = row["RoleId"] == DBNull.Value ? string.Empty : (string)row["RoleId"],
            RoleName = row["RoleName"] == DBNull.Value ? string.Empty : (string)row["RoleName"],

        };



    }


    //ADD BY DALI 

    private const string SelectDurectorsByLaboratoryQuery = "dbo.getDirectorsByLaboratory";
    public async Task<List<Member>> SelectDirectorsByLaboratory(string laboId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand selectLaboratoryByUserCommand = new SqlCommand(SelectDurectorsByLaboratoryQuery, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            selectLaboratoryByUserCommand.Parameters.AddWithValue("@LabId", laboId);

            SqlDataAdapter adapter = new(selectLaboratoryByUserCommand);
            DataSet set = new();
            adapter.Fill(set);

            await connection.OpenAsync();
            var targetLabTable = set.Tables[0];
            List<Member> members = new List<Member>();
           
            foreach (DataRow row in targetLabTable.Rows)
            {
                members.Add(getMemberFromDataRow(row));
            }
            return members;
        }
    }


    //================================================

}