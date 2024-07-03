using GLAB.Domains.Shared;

namespace GLAB.Domains.Models.Members;

public static class MemberError
{
    public static ErrorCode IdEmpty { get; } =
        new ErrorCode("MemberError.IdEmpty", "The member id cannot be empty");

    public static ErrorCode StatusEmpty { get; } =
        new ErrorCode("MemberError.StatusEmpty", "The member status cannot be empty");

    public static ErrorCode FirstNameEmpty { get; } =
        new ErrorCode("MemberError.FirstNameEmpty", "The first name cannot be empty");

    public static ErrorCode LastNameEmpty { get; } =
        new ErrorCode("MemberError.LastNameEmpty", "The last name cannot be empty");

    public static ErrorCode EmailEmpty { get; } =
        new ErrorCode("MemberError.EmailEmpty", "The email address cannot be empty");

    public static ErrorCode NICEmpty { get; } =
        new ErrorCode("MemberError.NICEmpty", "The NIC number cannot be empty");

    public static ErrorCode PhoneNumberEmpty { get; } =
        new ErrorCode("MemberError.PhoneNumberEmpty", "The phone number cannot be empty");

    public static ErrorCode PhotoEmpty { get; } =
        new ErrorCode("MemberError.PhotoEmpty", "The image cannot be empty");
}