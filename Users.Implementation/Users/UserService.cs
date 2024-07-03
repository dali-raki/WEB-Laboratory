using System.Security.Claims;
using GLAB.App.Users;
using GLAB.Domains.Models.Users;
using GLab.Impl.Services.Users;
using Users.Infra.Storages;
using Users.Implementation.Users;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Shared;
using GLAB.Infra.Storages.MembersStorages;
using GLAB.App.Memebers;
using GLAB.Domains.Models.Laboratories;
using Users.Domains.Users;
using Email.App;


namespace GLAB.Implementation.Users;

public class UserService : IUserService

{

    private readonly IUserStorage userStorage;
    private readonly IMemberService memberService;
    private PasswordHasher passwordHasher;
    private readonly IEmailService emailService;

    public UserService(IUserStorage userStorage, IMemberService memberService, IEmailService emailService)
    {
        this.userStorage = userStorage;
        this.memberService = memberService;
        this.emailService = emailService;
        passwordHasher = new PasswordHasher();
    }

    public async Task<User?> GetUserById(string userId)
    {
        try
        {
            var user = await userStorage.SelectUserById(userId);
            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            return user;
        }
        catch (Exception e)
        {
            throw new Exception($"Error retriving the user : {e.Message} ");
        }
    }

    public async Task<User?> GetUserByUserName(string userName)
    {
        return await userStorage.SelectUserByUserName(userName);
    }
        
    public async Task<bool> ValidatePassword(string userId, string password)
    {
        string hashedPassword = await userStorage.SelectUserPassword(userId);
        return passwordHasher.VerifyHashedPassword(hashedPassword, password);
    }

    public async Task CreateUser(User user)
    {
        validateUserForCreation(user);
        validateUserNameDoesNotExists(user.UserName);
        validateEmail(user.UserName);
        await userStorage.InsertUser(user);
    }

    private void validateUserNameDoesNotExists(string userName)
    {
        if (userStorage.SelectUserByUserName(userName).GetAwaiter().GetResult() is not null)
            throw new Exception($"Username {userName} already exists");
    }

    private void validateUserExists(string userId)
    {
        if (userStorage.SelectUserById(userId).GetAwaiter().GetResult() is null)
            throw new Exception($"User: {userId} doesn't exist");
    }

    private async Task<bool> validateEmail(string email)
    {

        IEmailValidator validator = new UnivEmailValidator();

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new Exception("Email addresse is mandatory");
        }

        bool isValid = await validator.ValidateEmailAsync(email);
        if (!isValid)
        {
            throw new Exception("Invalid Email addresse. You need to enter an email with this format: alphanumericChar@univ-.dz.");
        }

        return isValid;
    }
    private void validateUserForCreation(User user)
    {
        if (user is null)
            throw new Exception("No user is found");

        if (string.IsNullOrWhiteSpace(user.UserId) ||
            string.IsNullOrWhiteSpace(user.UserName) ||
            string.IsNullOrWhiteSpace(user.Password))
            throw new Exception("Error validating member");
    }

    public async Task ChangePassword(string userId, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("New password cannot be null or empty.", nameof(newPassword));

        string hashedPassword = passwordHasher.HashPassword(newPassword);
        Console.Write(newPassword);

        await userStorage.UpdateUserPassword(userId, hashedPassword);
    }

    public async Task<bool> SetUser(User user)
    {
        try
        {
            validateUserExists(user.UserId);
            bool isUserUpdated = await userStorage.UpdateUser(user);
            if (isUserUpdated == false)
    {
                throw new Exception("Error while updating member");
            }
        
            return isUserUpdated;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while updating user: {ex.Message}");
        }
    }

    public async Task<List<Claim>> getUserRoles(string userId)
    {
       return await userStorage.SelectUserRoles(userId);
    }

    public async ValueTask<Result> CreateUserTransactionMode(User user, Member member)
    {
         validateUserForCreation(user);
         validateUserNameDoesNotExists(user.UserName);
         await  validateEmail(user.UserName);

        try
        {
            List<ErrorCode> errorList =  memberService.ValidateMemberForInsert(member);

            if (errorList.Any())
                return Result.Failure(errorList);

            await userStorage.InsertUserTransactionMode(user, member);

            return Result.Succes;
        }
        catch (Exception ex)
        {
            return Result.Failure(new string[] { "An error occurred while inserting the member." });
        }


    }

    public async Task<Result> SetUserTransactionMode(User user, Member member)
    {
        try
        {
            validateUserExists(user.UserId);
            List<ErrorCode> errorList = await memberService.ValidateMemberForUpdate(member);
            if (errorList.Any())
                return Result.Failure(errorList.Select(e => e).ToList());

            bool isUserUpdated = await userStorage.UpdateUserTransactionMode(user, member);
            if (isUserUpdated == false)
            {
                throw new Exception("Error while updating user");
            }

            return Result.Succes;
        }
        catch (Exception ex)
        {
            return Result.Failure(new[] { "An error occurred while setting the member." });

        }
    }


    public async Task CreateAdmin(Laboratory lab)
    {
        String userId = Guid.NewGuid().ToString();
        UserStatus stateBlocked = UserStatus.Allowed;
        UserProfile userProfile = UserProfile.LabADmin;
        String password = TokenGenerator.GenerateToken(16);
        string hashedPassword = passwordHasher.HashPassword(password);

        User userToCreate = User.Create(userId, lab.Email, stateBlocked, hashedPassword, userProfile);
        await CreateUser(userToCreate);

        await sendAdminRegistrationEmailAsync(userToCreate, lab, password);


    }


    private async Task sendAdminRegistrationEmailAsync(User user, Laboratory lab, string password)
    {
        String URI = "https://localhost:7067/";

        String emailSubject = "Access Granted: Your Web Admin Credentials";

        String emailBody = lab.Name + " Admin," +
            "<br /> These are the necessary credentials to access your account: " +
            "<br /> Username: " + user.UserName + " " +
            "<br /> Password: " + password + " " +
            "<br /> Please keep this information secure and confidential " +
            "<br /> You can now log in to our application using the provided credentials " +
            "<br /> Simply follow this link to access the login page: " +
            "<br /> <a href=" + URI + ">Click here</a> " +
            "<br /> Once logged in, you will have to complete the neccessary laboratory information " +
            "<br /> Best regards" +
            "<br /> Laboratory Development Team.";
        try
        {
            await emailService.SendEmailAsync(user.UserName, emailSubject, emailBody);
        }
        catch (Exception e)
        {
            throw new Exception("Error occuried when sending confirm registration email");
        }
    }
}