using GLAB.Domains.Shared;

namespace Glab.Domains.Models.Roles
{
    public class RoleErrors
    {
        public static ErrorCode IdEmpty { get; } =
      new ErrorCode("RoleError.IdEmpty", "The Role id cannot be empty");

        public static ErrorCode NameEmpty { get; } =
        new ErrorCode("RoleError.NameEmpty", "The Role Name cannot be empty");



    }
}
