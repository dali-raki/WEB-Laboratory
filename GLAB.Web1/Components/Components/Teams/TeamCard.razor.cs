using GLAB.App.Memebers;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace GLAB.Web1.Components.Components.Teams;

partial class TeamCard
{
    private bool cardMenuVisible;
    private ElementReference teamCard;
    private bool moreDetailsShown;
    [Parameter] public Team targetTeam { get; set; }
    [Parameter] public EventCallback<Team> deleteTeamEvent { get; set; }
    [Parameter] public EventCallback<Team> AddMemberEvent { get; set; }

    [Inject] private IJSRuntime js { get; set; }

    private Member teamLeader;
    
    protected override async Task OnInitializedAsync()
    {
        teamLeader = targetTeam.Members.Find
            (member => member.MemberId.Equals(targetTeam.TeamLeaderId));
    }

    private void revealMenu()
    {
        cardMenuVisible = true;
    }

    private void closeMenu()
    {
        cardMenuVisible = false;
    }
    
    private async Task deleteTeam()
    {
       await deleteTeamEvent.InvokeAsync(targetTeam);
    }
    
    private async Task showMoreDetails()
    {
      await js.InvokeVoidAsync("displayDetailedTeamCard",teamCard);

      moreDetailsShown = true;
    }

    private  async Task goBack()
    {
        await js.InvokeVoidAsync("HideDetailedTeamCard",teamCard);
        
        moreDetailsShown = false; 
    }

    private async Task openAddMemberPopup()
    {
       await AddMemberEvent.InvokeAsync(targetTeam);
    }
    
}