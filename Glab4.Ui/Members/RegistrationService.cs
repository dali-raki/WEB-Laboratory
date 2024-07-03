using Email.App;
using Glab.App.Grades;
using Glab.Ui.Members.Models;
using GLab.Impl.Services.Users;
using GLAB.App.Memebers;
using GLAB.App.Teams;
using GLAB.App.Users;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Users;
using GLAB.Domains.Shared;
using System.Transactions;
using Users.Domains.Users;
using Users.Implementation.Users;

namespace Glab.Ui.Members
{
    public interface IRegistrationService
    {
        Task<Result> RegisterNewMember(CreateMemberModels memberModel);
        Task<Result> RegisterNewMemberTransactionMode(CreateMemberModels memberModel);
        ValueTask<bool> ConfirmMemberRegistration(ConfirmRegistrationModel confirmRegistrationModel, string userId, string token);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService userService;
        private readonly IMemberService memberService;
        private readonly IEmailService emailService;
        private PasswordHasher passwordHasher = new PasswordHasher();

        public RegistrationService(IUserService userService, IMemberService memberService, IEmailService emailService, ITeamService teamService)
        {
            this.userService = userService;
            this.memberService = memberService;
            this.emailService = emailService;
        }

        public async Task<Result> RegisterNewMember(CreateMemberModels memberModel)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                
                String userId = Guid.NewGuid().ToString();
                UserStatus stateBlocked = UserStatus.Bloqued;
                UserProfile userProfile = UserProfile.LabMember;
                User userToCreate = User.Create(userId, memberModel.Email, stateBlocked, userProfile);

                Member memberToCreate = new Member()
                {
                    MemberId = userId,
                    FirstName = memberModel.Nom,
                    LastName = memberModel.Prenom,
                    Email = memberModel.Email,
                    TeamId = memberModel.Equipe,
                    Status = MemberStatus.Bloqued,
                    NIC = "1111",
                    PhoneNumber = "34567890",
                    Image = new byte[] { 1 }
                };

                await userService.CreateUser(userToCreate);

                Result isMemberCreated = await memberService.CreateMember(memberToCreate);
                if (isMemberCreated.Successed == false)
                {
                    throw new Exception(isMemberCreated.Errors.ToString());
                }

                await sendRegistrationEmailAsync(memberToCreate);

                scope.Complete();
                return isMemberCreated;
            }
            catch (Exception e)
            {
                throw new Exception($"Echec de la création du nouveau membre : {e.Message}");
            }
            finally
            {
                scope.Dispose();
            }
        }

        private async Task sendRegistrationEmailAsync(Member member)
        {
            String URI = "https://localhost:7067/member/confirmRegistration";
            String token = TokenGenerator.GenerateToken(128);
            
            await userService.ChangePassword(member.MemberId, token);
        
            String emailSubject = "Confirm Registration";

            String emailBody =
                "Hello, " + member.LastName + "  " + member.FirstName +
                "<br />to complete your registration in our laboratory" +
                " click the link below " +
                "<br /><a href=" + URI + "/" + member.MemberId + "/" + token + ">Click here</a>";

          try
            {
            await emailService.SendEmailAsync(member.Email, emailSubject, emailBody);
            }
          catch (Exception e)
            {
                throw new Exception("Error occuried when sending confirm registration email");
            }
        }


        public async ValueTask<bool> ConfirmMemberRegistration(ConfirmRegistrationModel confirmRegistrationModel, string memberId, string token)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // validate id and token in uri
              
                bool isUserValide = validateUser(memberId, token);
                 if(isUserValide == false)
                {
                    throw new Exception("Invalid URI");
                }
                    bool isPasswordCorrect = confirmPassword(confirmRegistrationModel.Password, confirmRegistrationModel.ConfirmPassword);
                if (isPasswordCorrect == false)
                {
                    return isPasswordCorrect;
                }


                Member memberToUpdate = new Member()
                {
                    MemberId = memberId,
                    Status = MemberStatus.Active,
                    NIC = confirmRegistrationModel.NIC,
                    PhoneNumber = confirmRegistrationModel.PhoneNumber,
                    Image = confirmRegistrationModel.Image
                };                

                bool confirmedRegistration = true;
                User userToUpdate = User.Create(memberId, confirmedRegistration, UserStatus.Allowed);

                Result isUserUpdated = await userService.SetUserTransactionMode(userToUpdate, memberToUpdate);

                if (isUserUpdated.Successed == false)
                {
                    throw new Exception(isUserUpdated.Errors.ToString());
                }

                await userService.ChangePassword(memberId, confirmRegistrationModel.Password);

                scope.Complete();
                return isUserUpdated.Successed;
            }
            catch (Exception e)
            {
                throw new Exception($"Confirm member registration error {e.Message}");
            }
            finally
            {
                scope.Dispose();
            }
        }

        private bool confirmPassword(string password, string confirmPassword)
        {
            bool validatePassword = password.Equals(confirmPassword);
            return validatePassword;
        }

        private bool validateUser(String userId, String token)
        {
            try
            {
                var userToValidate = userService.GetUserById(userId);
                String userPassword = userToValidate.Result.Password;

                bool isTokenValid = passwordHasher.VerifyHashedPassword(userPassword, token);
                if (isTokenValid == false)
                {
                    throw new Exception("Token invalid");
                }

                return isTokenValid;

            }
            catch (Exception ex)
            {
                throw new Exception("Echec de Verification de token");
            }

        }

        public async Task<Result> RegisterNewMemberTransactionMode(CreateMemberModels memberModel)
        {
            try
            {

                String userId = Guid.NewGuid().ToString();
                UserStatus stateBlocked = UserStatus.Bloqued;
                UserProfile userProfile = UserProfile.LabMember;
                User userToCreate = User.Create(userId, memberModel.Email, stateBlocked, userProfile);

                Member memberToCreate = new Member()
                {
                    MemberId = userId,
                    FirstName = memberModel.Nom,
                    LastName = memberModel.Prenom,
                    Email = memberModel.Email,
                    TeamId = memberModel.Equipe,
                    GradeId = memberModel.GradeId,
                    Status = MemberStatus.Bloqued,
                    NIC = "1111",
                    PhoneNumber = "34567890",
                    Image = new byte[] { 1 }
                };


                Result isMemberCreated = await userService.CreateUserTransactionMode(userToCreate, memberToCreate);
                if (isMemberCreated.Successed == false)
                {
                    throw new Exception(isMemberCreated.Errors.ToString());
                }
                
                await sendRegistrationEmailAsync(memberToCreate);
                
                return isMemberCreated;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while creating a new member: {e.Message}");
            }

        }
    }
}
