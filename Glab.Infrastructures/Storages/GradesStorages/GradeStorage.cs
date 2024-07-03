using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Glab.Domains.Models.Grades;

namespace Glab.Infrastructures.Storages.GradesStorages
{
    public class GradeStorage : IGradeStorage
    {
        private readonly string connectionString;
        private const string selectGradesQuery = "select * from dbo.GRADES";
        private const string selectGradeByIdQuery = "SELECT * FROM GRADES WHERE GradeId = @aGradeId";
        private const string selectGradeByNameQuery = "SELECT * FROM GRADES WHERE GradeName = @aGradeName";

        public GradeStorage(IConfiguration configuration) =>
       connectionString = configuration.GetConnectionString("Redouane");

        private static Grade getGradeFromDataRow(DataRow row)
        {
            return new()
            {
                GradeId = (string)row["GradeId"],
                GradeName = (string)row["GradeName"],

            };
        }

        public async Task<List<Grade>> SelectGrades()
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(selectGradesQuery, connection);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            return (from DataRow row in ds.Rows select getGradeFromDataRow(row)).ToList();
        }
        public async Task<Grade?> SelectGradeById(string gradeId)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(selectGradeByIdQuery, connection);
            cmd.Parameters.AddWithValue("@aGradeId", gradeId);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            return ds.Rows.Count == 0 ? null : getGradeFromDataRow(ds.Rows[0]);
        }

        public async Task<Grade?> SelectGradeByName(string gradeName)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(selectGradeByNameQuery, connection);
            cmd.Parameters.AddWithValue("@aGradeName", gradeName);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            return ds.Rows.Count == 0 ? null : getGradeFromDataRow(ds.Rows[0]);
        }







    }
}
