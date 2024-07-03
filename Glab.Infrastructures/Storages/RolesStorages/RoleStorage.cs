using Glab.Domains.Models.Roles;
using GLAB.Domains.Models.Laboratories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Glab.Infrastructures.Storages.RolesStorages
{
    public class RoleStorage : IRoleStorage
    {
        private readonly string _connectionString;
        private const string _selectRolesQuery = "SELECT * FROM dbo.ROLES";
        private const string _selectRoleByIdQuery = "SELECT * FROM ROLES WHERE RoleId = @RoleId";
        private const string _selectRoleByNameQuery = "SELECT * FROM ROLES WHERE RoleName = @RoleName";
        private const string _insertRoleQuery = "INSERT INTO ROLES (RoleId, RoleName) VALUES (@RoleId, @RoleName)";
        private const string _updateRoleQuery = "UPDATE ROLES SET RoleName = @RoleName WHERE RoleId = @RoleId";
        private const string _deleteRoleQuery = "DELETE FROM ROLES WHERE RoleId = @RoleId";

        public RoleStorage(IConfiguration configuration) =>
            _connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane");

        private Role GetRoleFromDataRow(DataRow row) =>
            new Role
            {
                RoleId = row.Field<string>("RoleId"),
                RoleName = row.Field<string>("RoleName")
            };

        private async Task<List<Role>> ExecuteQueryAsync(string query, SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            var dataTable = new DataTable();
            using var dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataTable);

            return dataTable.Rows.Cast<DataRow>().Select(GetRoleFromDataRow).ToList();
        }

        public async Task<List<Role>> SelectRoles() =>
            await ExecuteQueryAsync(_selectRolesQuery, Array.Empty<SqlParameter>());

        public async Task<Role?> SelectRoleById(string roleId) =>
            (await ExecuteQueryAsync(_selectRoleByIdQuery, new[] { new SqlParameter("@RoleId", roleId) })).FirstOrDefault();

        public async Task<Role?> SelectRoleByName(string roleName) =>
            (await ExecuteQueryAsync(_selectRoleByNameQuery, new[] { new SqlParameter("@RoleName", roleName) })).FirstOrDefault();

        public async Task CreateRole(Role role)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(_insertRoleQuery, connection);
            cmd.Parameters.AddWithValue("@RoleId", role.RoleId);
            cmd.Parameters.AddWithValue("@RoleName", role.RoleName);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateRole(Role role)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(_updateRoleQuery, connection);
            cmd.Parameters.AddWithValue("@RoleId", role.RoleId);
            cmd.Parameters.AddWithValue("@RoleName", role.RoleName);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteRole(string roleId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(_deleteRoleQuery, connection);
            cmd.Parameters.AddWithValue("@RoleId", roleId);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        private const string assignLabDirectorQuery = "SetLaboratoryDirector";

        public async Task AssignLabDirector(string LaboratoryId, string DirectorId)
        {


            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(_deleteRoleQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@aLaboratoryId", LaboratoryId);
            cmd.Parameters.AddWithValue("@aDirectorId", DirectorId);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

    }
}