using Glab.Domains.Models.Grades;

namespace Glab.Infrastructures.Storages.GradesStorages
{
    public interface IGradeStorage
    {
        Task<List<Grade>> SelectGrades();
        Task<Grade?> SelectGradeById(string GradeId);
        Task<Grade?> SelectGradeByName(string GradeName);
    }
}
