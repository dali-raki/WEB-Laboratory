using GLAB.App.Accounts;
using GLAB.App.Users;
using GLAB.Domains.Models.Users;
using Users.Domains.Users;
//using GLAB.Api.Accounts;
//using GLAB.Api.Users;
//using GLAB.Domains.Models.Users;


namespace GLAB.Implementation.Accounts;

public class AccountService : IAccount
{
    private readonly IUserService userService;

    public AccountService(IUserService userService)
    {
        this.userService = userService;
        
    }

    public async Task<(IAccount.LoginStatus,User)> CheckCredentials(string username, string password)
    {
       
        
      var user= await userService.GetUserByUserName(username);

      if (user == null || user.State == UserStatus.Deleted)
          return (IAccount.LoginStatus.UserNotExists,null);

      if (user.State == UserStatus.Bloqued)
          return (IAccount.LoginStatus.UserBlocked,null);


      bool isPasswordCorrect = await userService.ValidatePassword(user.UserId, password);

      if (isPasswordCorrect)
      {
          return (IAccount.LoginStatus.CanLogin,user);  
      }

      return (IAccount.LoginStatus.WrongCredentials,null);
      
      
    }

    public async Task<bool> ChangePassword(string userid, string oldpassword, string newpassword)
    {
        throw new NotImplementedException();
    }
}