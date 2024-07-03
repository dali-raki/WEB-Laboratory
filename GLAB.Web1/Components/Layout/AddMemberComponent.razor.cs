using Glab.Ui.Members;
using Glab.Ui.Members.Models;
using GLAB.App.Memebers;
using GLAB.App.Teams;
using Glab.App.Roles;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;
using Glab.Domains.Models.Roles;
using System.Security.Cryptography;
using Glab.Domains.Models.Grades;
using Glab.App.Grades;
using GLAB.Domains.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;


namespace GLAB.Web1.Components.Layout
{
    public partial class AddMemberComponent
    {

        [Inject] private IRegistrationService registrationService { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private IMemberService memberService { get; set; }

        [Inject] private ITeamService teamService { get; set; }

        [Inject] private IRoleService roleService { get; set; }
        [Inject] private IGradeService gradeService { get; set; }
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private List<Team> teams;
        private List<Role> roles;
        private List<Grade> grades;

        private CreateMemberModels member = new CreateMemberModels();
 
        private string errorMessage = "Try Again";
        private string success = null;

        private List<Role> SelectedRoles;


        private void ToggleRoleSelection(Role role, ChangeEventArgs e)
        {

            role.selected = (bool)e.Value; // Update the selected state of the role

            if (role.selected)
            {
                SelectedRoles.Remove(role);
                Console.WriteLine("Role removed: " + role.RoleName);
            }
            else
            {
                SelectedRoles.Add(role);
                Console.WriteLine("Role added: " + role.RoleName);
            }
        }


        private async Task loadData()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string labId = auth.User.Claims.ToList().Find(claim => claim.Type == "LabId").Value;
            teams = await teamService.GetTeamsByLaboratory(labId);
            roles = await roleService.GetRoles();
            grades = await gradeService.GetGrades();
            
        }
       
        protected override async Task OnInitializedAsync()
        {
        
            await loadData();
        }

        private void createMember()
        {

          registrationService.RegisterNewMemberTransactionMode(member);
            success = "Member is Created";
        }
       
    }
}
