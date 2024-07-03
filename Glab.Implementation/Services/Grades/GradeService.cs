using Glab.App.Grades;
using Glab.Domains.Models.Grades;
using Glab.Infrastructures.Storages.GradesStorages;


namespace Glab.Implementation.Services.Grades
{
    public class GradeService : IGradeService
    {
        private readonly IGradeStorage gradeStorage;

        public GradeService(IGradeStorage gradeStorage)
        {
            this.gradeStorage = gradeStorage;
        }
        public async ValueTask<Grade?> GetGradeById(string GradeId)
        {
            return await gradeStorage.SelectGradeById(GradeId);
        }

        public async ValueTask<Grade?> GetGradeByName(string GradeName)
        {
            return await gradeStorage.SelectGradeByName(GradeName);
        }

        public async ValueTask<List<Grade>> GetGrades()
        {
            return await gradeStorage.SelectGrades();
        }
    }
}
