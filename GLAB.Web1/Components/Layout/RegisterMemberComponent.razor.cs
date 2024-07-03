using Glab.App.Grades;
using Glab.Domains.Models.Grades;
using Glab.Ui.Members;
using Glab.Ui.Members.Models;
using GLAB.App.Laboratories;
using GLAB.App.Memebers;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Models.Members;
using GLAB.Web1.Components.Members.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using GLAB.Domains.Models.Users;
using GLAB.App.Users;
using GLAB.App.Teams;
using GLAB.Domains.Models.Teams;


namespace GLAB.Web1.Components.Layout
{
    public partial class RegisterMemberComponent
    {

        [Inject] public IMemberService MemberService { get; set; }
        [Inject] public IGradeService gradeService { get; set; }
        [Inject] private IRegistrationService registrationService { get; set; }
        [Inject] private ILabService laboratoryService { get; set; }
        [Inject] private IUserService userService { get; set; }
        [Inject] private ITeamService teamService { get; set; }
        private Laboratory laboratory;
        private Member member;
        private User user;
        private Team team;
        [Parameter] public string id { get; set; }
        [Parameter] public string token { get; set; }
        [Parameter] public Action OnRegistered { get; set; }
        private ConfirmRegistrationModel confirmRegistrationModel = new ConfirmRegistrationModel();
        private void photoChanged(byte[] image) => confirmRegistrationModel.Image = image;
        private List<Grade> Grades;
        private Grade grade;
        private bool loaded;
        private bool hasError = false;
        private string errorMessage = string.Empty;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        protected override async Task OnInitializedAsync()
        {

            user = await userService.GetUserById(id);
            member = await MemberService.GetMemberByEmail(user.UserName);
            team = await teamService.GetTeamById(member.TeamId);
            Console.Write(team.LaboratoryId);
           //laboratory = await laboratoryService.GetLaboratoryById(team.LaboratoryId);
           // Console.Write(laboratory.Name);
            
        }
        private void regsiterMember()
            {
                registrationService.ConfirmMemberRegistration(confirmRegistrationModel, id, token);
                OnRegistered.Invoke();
            }
    }

}

