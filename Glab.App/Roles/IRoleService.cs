using Glab.Domains.Models.Roles;


namespace Glab.App.Roles
{
    public interface IRoleService
    {
        ValueTask<List<Role>> GetRoles();
        ValueTask<Role?> GetRoleById(string RoleId);
        ValueTask<Role?> GetRoleByName(string RoleName);
        Task AssignLabDirector(string LaboratoryId, string DirectorId);
    }
}
