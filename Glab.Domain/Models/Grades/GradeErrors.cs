using GLAB.Domains.Shared;

namespace Glab.Domains.Models.Grades
{
    public class GradeErrors
    {
        public static ErrorCode IdEmpty { get; } =
    new ErrorCode("GradeError.IdEmpty", "The Grade id cannot be empty");

        public static ErrorCode NameEmpty { get; } =
        new ErrorCode("GradeError.NameEmpty", "The Grade Name cannot be empty");
    }
}
