namespace GLAB.Domains.Models.Users;

public class UserClaims
{
    private string UserId { get; set; }

    private string Email { get; set; }

    private List<ApplicationRole> Roles;

    public UserClaims( string userId, string email,List<ApplicationRole> roles)
    {
        
        Roles = roles;
        UserId = userId;
        Email = email;
        
    }
}

