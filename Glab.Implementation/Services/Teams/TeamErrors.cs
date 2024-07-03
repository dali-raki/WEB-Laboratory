using GLAB.Domains.Shared;


namespace GLAB.Implementation.Services.Teams
{
    public class TeamErrors
    {
        public static ErrorCode IdExist { get; } =
    new ErrorCode("TeamErrors.IdExist", "The Team's Id is Already Exist");


    }
}