using Glab.Domains.Models.Roles;

namespace Glab.Infrastructures.Storages.RolesStorages
{
    public interface IRoleStorage
    {
        Task<List<Role>> SelectRoles();
        Task<Role?> SelectRoleById(string RoleId);
        Task<Role?> SelectRoleByName(string RoleName);
        Task CreateRole(Role role);
        Task UpdateRole(Role role);
        Task DeleteRole(string roleId);
        Task AssignLabDirector(String LaboratoryId,String DirectorId);
    }
}
