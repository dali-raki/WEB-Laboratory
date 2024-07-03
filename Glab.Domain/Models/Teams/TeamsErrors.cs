using GLAB.Domains.Shared;

namespace GLAB.Domains.Models.Teams;

public static class TeamErrorCodes
{
    public static ErrorCode IdEmpty { get; } =
        new ErrorCode("TeamError.IdEmpty", "The  identifier cannot be empty");

    public static ErrorCode NameEmpty { get; } =
        new ErrorCode("TeamError.NameEmpty", "The  name cannot be empty");

    public static ErrorCode StatusEmpty { get; } =
        new ErrorCode("TeamError.StatusEmpty", "The  status cannot be empty");
}