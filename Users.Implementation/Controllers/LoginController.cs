using System.Security.Claims;
using GLAB.App.Accounts;
using GLAB.App.Laboratories;
using GLAB.App.Users;
using GLAB.App.Memebers;
using Glab.App.Roles;
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
    private readonly ILabService laboratoryService;

    public IAccount AccountService => _accountService;

    public IUserService UserService => _userService;


    private IRoleService _roleService;

    public LoginController(IAccount accountService,IUserService userService,IRoleService roleService,ILabService laboratoryService)
    {
        _accountService = accountService;
        this._userService = userService;
        this._roleService = roleService;
        this.laboratoryService = laboratoryService;

    }
    
    
    [HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("Login")]
    
    public async Task<IActionResult> Login( [FromForm] string username, [FromForm] string password)
    {
        var (Status,user) =  await AccountService.CheckCredentials(username, password);
        if(Status!=null && user!=null)
        {
            if (Status == IAccount.LoginStatus.CanLogin)
            {
                
                List<Claim> claims =  await UserService.getUserRoles(user.UserId);
            
                try
                {
                    
                    var userTargetLab =await laboratoryService.GetLaboratoryByUser(user.UserId);
                    
                    if (userTargetLab != null)
                    {
                        claims.Add(new Claim("LabId",userTargetLab.LaboratoryId));
                    }
                
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();

                
                
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                
                    var Profile=claims.Find(claim => claim.Type.Equals("Profile"));
                
                    switch (Profile.Value)
                    {
                    
                        case "1":
                            return Redirect("/Labs/CreateManager");
                            break;
                    
                        case "2":
                            return Redirect("/Labs/CreateAdmin");
                            break;
                    
                        case "3":
                            return Redirect("/Labs/CreateAdmin");
                            break;
                    
                    }
                
                
                
                    return Redirect("/");
                
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
           
        }
      
            return Redirect("/");

        
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
    
    [HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("Logout")]
    
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return Redirect("/");

    }
   
    
    
}