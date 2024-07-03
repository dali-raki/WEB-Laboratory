using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glab.Domains.Models.Faculties;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.Extensions.Configuration;

namespace Glab.Infrastructures.Storages.FacultiesStorages
{
    public class FacultyStorage:IFacultyStorage
    {
        private readonly string connectionString;

        public FacultyStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane");
        }

        public async Task<List<Faculty>> SelectFaculties()
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new("Select * from VFaculty", connection);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            return (from DataRow row in ds.Rows select getFacultyFromDataRow(row)).ToList();
        }

        private Faculty getFacultyFromDataRow(DataRow row)
        {
            return new Faculty
            {
                FacultyId = row["FacultyId"] != DBNull.Value ? row["FacultyId"].ToString() : string.Empty,
                Name = row["Name"] != DBNull.Value ? row["Name"].ToString() : string.Empty,
                Acronyme = row["Acronyme"] != DBNull.Value ? row["Acronyme"].ToString() : string.Empty,

            };
        }
    }
}
