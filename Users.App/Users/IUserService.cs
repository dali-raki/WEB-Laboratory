using System.Security.Claims;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Users;
using GLAB.Domains.Shared;

namespace GLAB.App.Users;

public interface IUserService 
{
    Task<User?> GetUserById(string userId);
    
    Task<User?> GetUserByUserName(string userName);

    Task<List<Claim>> getUserRoles(string userUserId);

    Task CreateUser(User user);

    ValueTask<Result> CreateUserTransactionMode(User user, Member member);
    Task<Result> SetUserTransactionMode(User user, Member member);

    Task<bool> SetUser(User user);

    Task<bool> ValidatePassword(string userId , string userPassword);

    Task ChangePassword(string userId, string newPassword);

    Task CreateAdmin(Laboratory lab);

}