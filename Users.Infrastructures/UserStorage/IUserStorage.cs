

using System.Security.Claims;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Users;

namespace Users.Infra.Storages;

public interface IUserStorage
{
    ValueTask<User?> SelectUserById(string userId);

    ValueTask<User> SelectUserByUserName(string userName);

    ValueTask<string> SelectUserPassword(string userId);

    ValueTask<bool> InsertUser(User user);
    
    ValueTask<bool> UpdateUserPassword(string userId, string newPassword);

    ValueTask<bool> UpdateUser(User user);

    Task<List<Claim>> SelectUserRoles(string userId);
    ValueTask<bool> InsertUserTransactionMode(User user, Member member);
    ValueTask<bool> UpdateUserTransactionMode(User user, Member member);

    
}