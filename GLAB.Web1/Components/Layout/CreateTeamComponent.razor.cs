
using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLAB.App;
using GLAB.Domains.Models.Laboratories;
using GLAB.Web1.Components.Teams.Models;
using GLAB.App.Teams;
using GLAB.App.Laboratories;
using GLAB.Domains.Models.Laboratories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using String = System.String;
using GLAB.App.Memebers;
using GLAB.Domains.Models.Members;
using GLAB.Implementation.Services.Laboratories;


namespace GLAB.Web1.Components.Layout
{
    public partial class CreateTeamComponent
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject] public ITeamService createTeam { get; set; }
        [Inject] public ILabService laboratoryService { get; set; }
        [Inject] public IMemberService memberService { get; set; }
    
        
        private bool loaded = false;

        private List<Laboratory> Laboratories;
        private Laboratory laboratory;
        private List<Member> members;
        private Team infoteam;
        private List<Member> members2 = new();
        private CreateTeamModel teamModel = new CreateTeamModel();
        private bool hasError = false;
        private string errorMessage = string.Empty;
        private string success = null;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private  async Task loadData()
        {

            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string labId = auth.User.Claims.ToList().Find(claim => claim.Type == "LabId").Value;
            members = await memberService.GetMembers();

            foreach(Member member in members)


            {
                infoteam = await createTeam.GetTeamById(member.TeamId);

                if (infoteam.LaboratoryId != null)
                {
                    if (infoteam.LaboratoryId.Equals(labId))
                    {
                        if (member.GradeId is not null)
                        {
                            if (member.GradeId.Equals("1") || member.GradeId.Equals("2"))
                            {
                                members2.Add(member);
                            }
                        }
                        else { Console.WriteLine(member.MemberId); }
                    }
                }
            }

            members = members2;
            
        }

   
        protected  override async Task OnInitializedAsync()
        {
           await  loadData();

           
          
            try
            {
                loaded = false;
                
                
                StateHasChanged();
                
                loaded = true;
            }
            catch (Exception e)
            {
                errorMessage = $"ERROR RETRIEVING LABORATORIES: {e.Message}";
                
                hasError = true;
            }
        }
        private async Task CreateTeam()
        {

            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string labId = auth.User.Claims.ToList().Find(claim => claim.Type == "LabId").Value;

           
           
            try
            {
             

                Team teamtocreate = new Team()
                {
                    LaboratoryId = labId,
                    TeamId = Guid.NewGuid().ToString(),
                    TeamName = teamModel.TeamName,
                    Status = TeamStatus.Blocked,
                    TeamAcronyme= teamModel.TeamAcronyme,
                    TeamLeaderId = teamModel.TeamLeaderId



                };

                await createTeam.CreateTeam(teamtocreate);
                
                success = "The Team is Created";
                
                var member=members.Find(member => member.MemberId ==teamModel.TeamLeaderId);
                
                if (member!=null)
                {
                    member.TeamId = teamtocreate.TeamId;
                    await memberService.updateMember(member);
                }
         
               

            }
            catch (Exception e)
            {
                errorMessage = $"ERROR OF CREATIOG THE TEAM : {e.Message}";
                hasError = true;
            }



        }


    }
}