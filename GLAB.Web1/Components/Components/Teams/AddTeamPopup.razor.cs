using GLAB.App.Memebers;
using GLAB.App.Teams;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GLAB.Web1.Components.Components.Teams;

partial class AddTeamPopup
{
    [Inject] private ITeamService teamService { get; set; }
    [Inject] private IMemberService memberService { get; set; }
    [Inject] private AuthenticationStateProvider auth { get; set; }
    [Parameter] public EventCallback exitEvent { get; set; }
    [Parameter] public EventCallback<Team> CreateTeamEvent { get; set; }
    public bool AddTeamSuccess { get; set; }
    private List<Member> potentialLeaders=new ();
    private Team teamToCreate = new Team();
    private List<Member> members = new();
    private Guid selectedLeaderId;
    private bool isLoading=true;
    private List<string> errors=new List<string>();


    protected override async Task OnInitializedAsync()
    {
        Thread thread = new Thread(async () =>
        {
         
            await InvokeAsync(() =>
            {
                isLoading = true;
                StateHasChanged();
            });
            
            members = await memberService.GetMembers();

            foreach(Member member in members)
            { if(member.GradeId is not null) { 
                    if (member.GradeId.Equals("1") || member.GradeId.Equals("2") )
                    {
                        potentialLeaders.Add(member);
                    }
                }
                else { Console.WriteLine( member.MemberId); }
            }
            
            members = potentialLeaders;

            await InvokeAsync(() =>
            {
                isLoading = false;
                StateHasChanged();
            });
        });
        thread.Start();
        
    }

    private async Task Exit()
    {
        await exitEvent.InvokeAsync();
    }

    private async Task SubmitNewTeam()
    {
        try
        {
            if (selectedLeaderId == null || String.IsNullOrEmpty(teamToCreate.TeamAcronyme) || String.IsNullOrEmpty(teamToCreate.TeamName))
            {
                if (String.IsNullOrEmpty(teamToCreate.TeamAcronyme))
                {
                    errors.Add("Please Input The Team Acronym");
                }

                if (String.IsNullOrEmpty(teamToCreate.TeamName))
                {
                    errors.Add("Please Input The Team Name"); 
                }

                if (selectedLeaderId == null)
                {
                    errors.Add("Please Select A Team Leader");
                }
                await Task.Delay(3000);
                errors.Clear();
                return;
            }

            var teamLeader = potentialLeaders.Find(member => member.MemberId.Equals(selectedLeaderId.ToString()) );
            
            teamToCreate.TeamId = Guid.NewGuid().ToString();
            teamToCreate.TeamLeaderId =teamLeader.MemberId.ToString();
            teamToCreate.Members.Add(teamLeader);
            
            var authState =await auth.GetAuthenticationStateAsync();
           var labIdClaim= authState.User.Claims.First(claim => claim.Type.Equals("LabId"));
           var labId=labIdClaim.Value;
           if(String.IsNullOrEmpty(labId)) return;
           teamToCreate.LaboratoryId = labId;
        
            await teamService.CreateTeam(teamToCreate);
            
            await CreateTeamEvent.InvokeAsync(teamToCreate);
        }
        catch (Exception e)
        {
            
            await CreateTeamEvent.InvokeAsync(null);
            throw e;
        }
       
    }

    private void ClearFields()
    {
        teamToCreate.TeamName = "";
        teamToCreate.TeamAcronyme = "";
    }
}