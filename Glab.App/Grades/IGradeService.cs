using Glab.Domains.Models.Grades;


namespace Glab.App.Grades
{
    public interface IGradeService
    {
        ValueTask<List<Grade>> GetGrades();
        ValueTask<Grade?> GetGradeById(string GradeId);
        ValueTask<Grade?> GetGradeByName(string GradeName);
    }
}
