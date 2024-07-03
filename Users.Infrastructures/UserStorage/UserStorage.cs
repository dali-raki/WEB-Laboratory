using System.Data;
using System.Net.NetworkInformation;
using System.Security.Claims;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using GLAB.Domains.Models.Users;
//using GLab.Impl.Services.Users;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Users.Domains.Users;

namespace Users.Infra.Storages
{
    public class UserStorage : IUserStorage
    {
        private string connectionString;

        private const string insertUserCommand =
      "Insert USERS(UserId, UserName, Password, Status, UserProfile, ConfirmedRegistration) VALUES(@aId, @aUserName, @aPassword, @aStatus, @aUserProfile, @aConfirmedRegistration)";

        private const string SelectUserByIdCommand =
            "Select * from USERS where UserId = @aUserId";

        private const string SelectUserPasswordCommand =
                "Select Password from USERS where UserId = @aUserId";

        private const string SelectUserByUserNameCommand =
            "Select * from USERS where UserName = @aUserName";

        private const string updateUserPasswordCommand =
            "UPDATE USERS SET Password = @aPassword WHERE UserId = @aUserId";

        private const string updateUserCommand =
            "UPDATE USERS SET" +
            " Status = @aStatus," +
            " ConfirmedRegistration = @aConfirmedRegistration" +
            " WHERE UserId = @aUserId";

        private const string selectUserRolesProcedure = "dbo.GetClaims";


        public UserStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("IdentityDB");
        }

        public async ValueTask<User?> SelectUserById(string userId)

        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(SelectUserByIdCommand, connection);
            cmd.Parameters.AddWithValue("@aUserId", userId);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            if (ds.Rows.Count == 0)
                return null;

            return User.Create(
               (string)ds.Rows[0]["UserId"],
               (string)ds.Rows[0]["UserName"],
               (UserStatus)ds.Rows[0]["Status"],
               (string)ds.Rows[0]["Password"],
               (UserProfile)ds.Rows[0]["UserProfile"]

           );

        }

        public async ValueTask<string> SelectUserPassword(string userId)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(SelectUserPasswordCommand, connection);
            cmd.Parameters.AddWithValue("@aUserId", userId);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            if (ds.Rows.Count == 0)
                return default;

            return (string)ds.Rows[0]["Password"];
        }

        public async ValueTask<User> SelectUserByUserName(string userName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new(SelectUserByUserNameCommand, connection);
                cmd.Parameters.AddWithValue("@aUserName", userName);

                DataTable ds = new();
                SqlDataAdapter da = new(cmd);

                connection.Open();

                da.Fill(ds);

                if (ds.Rows.Count == 0)
                    return null;

                var UserId = (string)ds.Rows[0]["UserId"];
                var UserName = (string)ds.Rows[0]["UserName"];
                var state = (UserStatus)ds.Rows[0]["Status"];

                return User.Create(
                   (string)ds.Rows[0]["UserId"],
                   (string)ds.Rows[0]["UserName"],
                   (UserStatus)ds.Rows[0]["Status"],
                   (string)ds.Rows[0]["Password"],
                   (UserProfile)ds.Rows[0]["UserProfile"]

               );
            }

        }

        public async ValueTask<bool> InsertUser(User user)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(insertUserCommand, connection);
            cmd.Parameters.AddWithValue("@aId", user.UserId);
            cmd.Parameters.AddWithValue("@aUserName", user.UserName);
            cmd.Parameters.AddWithValue("@aPassword", user.Password);
            cmd.Parameters.AddWithValue("@aStatus", user.State);
            cmd.Parameters.AddWithValue("@aUserProfile", user.userProfile);
            cmd.Parameters.AddWithValue("@aConfirmedRegistration", user.ConfirmedRegistration);

            connection.Open();

            int insertedRows = await cmd.ExecuteNonQueryAsync();
            return (insertedRows > 0);
        }

        public async ValueTask<bool> UpdateUserPassword(string userId, string newPassword)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(updateUserPasswordCommand, connection);
            cmd.Parameters.AddWithValue("@aUserId", userId);
            cmd.Parameters.AddWithValue("@aPassword", newPassword);

            connection.Open();
            int updatedRows = await cmd.ExecuteNonQueryAsync();
            return (updatedRows > 0);
        }

        public async ValueTask<bool> UpdateUser(User user)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(updateUserCommand, connection);
            cmd.Parameters.AddWithValue("@aUserId", user.UserId);
            cmd.Parameters.AddWithValue("@aConfirmedRegistration", user.ConfirmedRegistration);
            cmd.Parameters.AddWithValue("@aStatus", user.State);

            connection.Open();
            int updatedRows = await cmd.ExecuteNonQueryAsync();
            return (updatedRows > 0);
        }


        public async Task<List<Claim>> SelectUserRoles(string userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(selectUserRolesProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);
                await connection.OpenAsync();

                DataSet ds = new DataSet();

                SqlDataAdapter da = new(cmd);

                da.Fill(ds);

                List<Claim> claims = new List<Claim>();

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    DataRow userCredentials = ds.Tables[0].Rows[0];
                    claims.Add(new Claim("Profile", userCredentials["UserProfile"].ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, userCredentials["UserName"].ToString()));
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count != 0)
                {
                    DataRow userCredentials = ds.Tables[1].Rows[0];
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, userCredentials["MemberId"].ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, userCredentials["Email"].ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, userCredentials["FirstName"].ToString()));
                    claims.Add(new Claim(ClaimTypes.Surname, userCredentials["LastName"].ToString()));
                    claims.Add(new Claim(ClaimTypes.MobilePhone, userCredentials["PhoneNumber"].ToString()));
                }

                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count != 0)
                {
                    DataTable userRoles = ds.Tables[2];

                    foreach (DataRow role in userRoles.Rows)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role["RoleName"].ToString()));

                    }
                }

                return claims;
            }
        }


        public String insertMemberStoredProcedure = "[dbo].[InsertMember]";

        public async ValueTask<bool> InsertUserTransactionMode(User user, Member member)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(insertMemberStoredProcedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", user.UserId);
            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Status", user.State);
            cmd.Parameters.AddWithValue("@FirstName", member.FirstName);
            cmd.Parameters.AddWithValue("@LastName", member.LastName);
            cmd.Parameters.AddWithValue("@TeamId", member.TeamId);
            cmd.Parameters.AddWithValue("@UserProfile", user.userProfile);
            cmd.Parameters.AddWithValue("@GradeId", member.GradeId);


            connection.Open();
            int insertedRows = await cmd.ExecuteNonQueryAsync();
            return (insertedRows > 0);
        }

        public String updateMemberStoredProcedure = "[dbo].[UpdateMember]";

        public async ValueTask<bool> UpdateUserTransactionMode(User user, Member member)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(updateMemberStoredProcedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", user.UserId);
            cmd.Parameters.AddWithValue("@Status", user.State);
            cmd.Parameters.AddWithValue("@ConfirmedRegistration", user.ConfirmedRegistration);
            cmd.Parameters.AddWithValue("@NIC", member.NIC);
            cmd.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
            cmd.Parameters.AddWithValue("@Image", member.Image);

            connection.Open();
            int insertedRows = await cmd.ExecuteNonQueryAsync();
            return (insertedRows > 0);
        }
    }
}