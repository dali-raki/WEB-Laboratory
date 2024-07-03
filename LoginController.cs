using System.Security.Claims;
using GLAB.App.Accounts;
using GLAB.App.Users;
using GLAB.App.Memebers;


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;


namespace Users.Controllers;
[Microsoft.AspNetCore.Mvc.Route("/Login")]
[ApiController]

public class LoginController : ControllerBase
{
    
    private readonly IAccount _accountService;

    private readonly IUserService _userService;
    
    private readonly IMemberService _memberService;

    public IAccount AccountService => _accountService;

    public IUserService UserService => _userService;

    public IMemberService MemberService => _memberService;

    public LoginController(IAccount accountService,IUserService userService)
    {
        _accountService = accountService;
        this._userService = userService;
        
    }
    
    
    [HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("Login")]
    
    public async Task<IActionResult> Login( [FromForm] string username, [FromForm] string password)
    {
        var (Status,user) =  await AccountService.CheckCredentials(username, password);
        
        if (Status == IAccount.LoginStatus.CanLogin)
        {
            
        var roles=  await UserService.getUserRoles(user.UserId);
         
            try
            {
                var Claims = new List<Claim>()
                {
                new Claim(ClaimTypes.Email,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.UserId),
                new Claim(ClaimTypes.Name,user.UserName),
                };
                
               
                Claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role,role.RoleName)));
                
                var identity = new ClaimsIdentity(Claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                return Redirect("/Labs/Create");
                
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        else
        {
            return Redirect("/Labs/Create");
        }
        
    }

    private string checkStatus(IAccount.LoginStatus status)
    {
        if (status == IAccount.LoginStatus.UserNotExists || status == IAccount.LoginStatus.UserBlocked)
        {
            return "User blocked Or Doesn't Exist";
        }
        else
        {
            return "Wrong Credentials";
        }
        
    }
    

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return Redirect("/");

    }
   
    
    
}