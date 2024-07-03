using System.Data.SqlTypes;
using GLAB.App.Laboratories;
using GLAB.App.Teams;
using GLAB.Domains.Models.Teams;
using GLAB.Implementation.Services.Teams;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace GLAB.Web1.Components.Components.Teams;

partial class DisplayTeams
{
    private List<Team> teams;
    private List<Team> searchedTeams=new ();
    private string searchedTeamName;
    private ElementReference searchBar;
    private Timer searchTimer;
    private bool confirmDialogShown;
    private Team targetTeam;
    private bool detailedCardVisible,addTeamPopupVisible;
    private bool deleteTeamSuccess;
    private bool isLoading;
    private bool addTeamSuccess;
    private bool isAddMemberVisible;
    private bool addMemberSuccess;


    [Inject] private ITeamService teamService { get; set; }
    [Inject] private IJSRuntime js { get; set; }
    
    [Inject] private AuthenticationStateProvider auth { get; set; }

    
    protected override async Task OnInitializedAsync()
    {
       var authState = await auth.GetAuthenticationStateAsync();
  
       if (authState != null)
       {
         var labClaim=  authState.User.Claims.FirstOrDefault(claim => claim.Type.Equals("LabId"));
         var labId=  labClaim != null ? labClaim.Value:null;
         
         if (labId != null)
         {
             new Thread(async o =>
             {
                 isLoading = true;
                 teams = await teamService.GetTeamsByLaboratory(labId);
                 searchedTeams = teams;
                 isLoading = false;
                 InvokeAsync(() =>
                 {
                    StateHasChanged();
                 });
                 
             } ).Start();
             
         }
         
       }
    
    }
    
    private  void  TimerCallback(Object o)
    {
         search();

    }

    private async Task search()
    {
        
        searchedTeams = teams.FindAll(team => team.TeamName.ToUpper().Contains(searchedTeamName.ToUpper()));
       await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    

    private async Task searchTeam()
    {
        
        if (String.IsNullOrEmpty(searchedTeamName))
        {
            searchedTeams = teams;
            return;
        }
        
        if (searchTimer != null)
        {
           await searchTimer.DisposeAsync();
          
        }
        
        searchTimer = new Timer(TimerCallback, null, 0, 300);

    }

 

    private async Task deleteTeam(Team team)
    {
        targetTeam = team;
        confirmDialogShown = true;
    }

    private async Task deleteTeamConfirmation(Object response)
    {
        
            new Thread(async o =>
            {
                try
                {
                    if(response is Team team)
                    {
                        bool success=  await teamService.RemoveTeam(team);
                        
                        if (success)
                        {
                            teams.Remove(team);
                           await InvokeAsync(() =>
                            {
                                deleteTeamSuccess = true;
                                StateHasChanged();
                            });

                        }
                    
                    }
                    
                    await js.InvokeVoidAsync("hideConfirmDialog");
                    await InvokeAsync(() =>
                    {
                        confirmDialogShown = false;
                        StateHasChanged();
                    });
                    
                    Thread.Sleep(2800);
                    
                    await InvokeAsync(() =>
                    {
                        deleteTeamSuccess = false;
                        StateHasChanged();
                    });
                   

                }
                catch (Exception e)
                {
                    throw e;
                }
                
                
            }).Start();
            
        
    }

  

    private void showAddTeamPopup()
    {
        addTeamPopupVisible = true;
    }
    private async Task closeDetailedCardView()
    {
        detailedCardVisible = false;
    }

    private async Task closeAddTeamPopup()
    {
        await js.InvokeVoidAsync("HideAddTeamPopup");
        await Task.Delay(600);
        
        addTeamPopupVisible = false;
    }

    private async Task ManageTeamCreation(Team createdTeam)
    {
        if (createdTeam != null)
        {
            teams.Add(createdTeam);
            searchedTeams = teams;
            addTeamSuccess = true;
            addTeamPopupVisible = false;
            await Task.Delay(10000);
            addTeamSuccess = false;
        }
    }

    private async Task OpenAddMemberPopup(Team team)
    {
        targetTeam = team;
        
        isAddMemberVisible = true;
    }

    private async Task closeAddMember()
    {
        isAddMemberVisible = false;
    }

    private async Task AddMemberSuccess()
    {
        addMemberSuccess = true;
        isAddMemberVisible = false;
    }
}