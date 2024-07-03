using Glab.App.Roles;
using Glab.Domains.Models.Roles;
using Glab.Infrastructures.Storages.RolesStorages;



namespace Glab.Implementation.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleStorage roleStorage;

        public RoleService(IRoleStorage roleStorage)
        {
            this.roleStorage = roleStorage;
        }

        public  Task AssignLabDirector(string LaboratoryId, string DirectorId)
        {
            return  roleStorage.AssignLabDirector(LaboratoryId, DirectorId);
        }

        public async ValueTask<Role?> GetRoleById(string RoleId)
        {
          /*  List<ErrorCode> errorList = await validateRoleIfNotExist(RoleId);*/

           /* if (! errorList.Any())*/
            return  await roleStorage.SelectRoleById(RoleId);


        }


        // missing validation methodRole ExistById 

/*        private async Task<List<ErrorCode>> validateRoleIfNotExist(string roleId)
        {
            bool roleExists = await roleStorage.RoleExistsById(roleId);
            if (!roleExists)
                throw new Exception("This Role Id is Not exists.");

            return new List<ErrorCode>();
        }
*/
        public async ValueTask<Role?> GetRoleByName(string RoleName)
        {
            return await roleStorage.SelectRoleById(RoleName);
        }

        public async ValueTask<List<Role>> GetRoles()
        {
            return await roleStorage.SelectRoles();
        }
    }
}
