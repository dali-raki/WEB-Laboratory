namespace GLAB.Domains.Models.Users;

public class ApplicationRole
{
    public string RoleId { get; set; }   
    
    public string RoleName { get; set; }

    public ApplicationRole(string roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
    }
}