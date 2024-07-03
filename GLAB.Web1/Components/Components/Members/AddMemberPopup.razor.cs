using Glab.App.Grades;
using GLAB.App.Memebers;
using Glab.App.Roles;
using GLAB.App.Teams;
using Glab.Domains.Models.Grades;
using GLAB.Domains.Models.Members;
using Glab.Domains.Models.Roles;
using GLAB.Domains.Models.Teams;
using Glab.Ui.Members;
using Glab.Ui.Members.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GLAB.Web1.Components.Components.Members;

partial class AddMemberPopup
{
     [Inject] private IRegistrationService registrationService { get; set; }
   
           [Inject]
           private NavigationManager navigationManager { get; set; }
           [Inject]
           private IMemberService memberService { get; set; }
   
           [Inject] private ITeamService teamService { get; set; }
   
           [Inject] private IRoleService roleService { get; set; }
           [Inject] private IGradeService gradeService { get; set; }
           
           [Parameter] public EventCallback CloseAddMemberPopupEvent { get; set; }
           
           [Parameter] public Team targetTeam { get; set; }
           [Parameter] public EventCallback AddMemberSuccess { get; set; } 
           
           private List<Team> teams=new ();
           private List<Role> roles=new ();
           private List<Grade> grades=new ();
   
           private CreateMemberModels member = new CreateMemberModels();
   
           private bool hasError = false;
           private string errorMessage = string.Empty;
           private string success = string.Empty;
   
           private List<Role> SelectedRoles;
           
           private bool isLoading;


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
               new Thread(async o =>
               {
                   await InvokeAsync(() =>
                   {
                       isLoading = true;
                       StateHasChanged();
                   });
                   
                   teams = await teamService.GetTeams();
                   roles = await roleService.GetRoles();
                   grades = await gradeService.GetGrades();
                   
                  await InvokeAsync(() =>
                   {
                       isLoading = false;
                       StateHasChanged();
                   });
               }).Start();
            
   
           }
          
           protected override async Task OnInitializedAsync()
           {
           
               await loadData();
           }
   
           private async Task createMember()
           {
               member.Equipe = targetTeam.TeamId;
               
              var result=  await registrationService.RegisterNewMemberTransactionMode(member);
              
              if (result.Successed)
              {
                  await AddMemberSuccess.InvokeAsync();
              }
              
           }
          
           private void ClearFields()
           {
               member.Nom = "";
               member.Prenom = "";
               member.Email = "";
           }

           private async Task Exit()
           {
               await CloseAddMemberPopupEvent.InvokeAsync();
           }
           
           
}