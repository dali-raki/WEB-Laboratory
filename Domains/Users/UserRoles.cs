namespace GLAB.Domains.Models.Users;

public class UserRoles
{
    public List<ApplicationRole> userRoles { get; set; }

    public UserRoles(List<ApplicationRole> userRoles)
    {
        this.userRoles = userRoles;
    }
}