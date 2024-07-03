using GLAB.App.Memebers;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GLAB.Web1.Components.Components.Teams;

partial class DetailedTeamCard
{
    private bool cardMenuVisible;
    private ElementReference teamCard;
    private bool expanded;
    private bool membersLoaded;
    [Parameter] public Team targetTeam { get; set; }
    [Parameter] public EventCallback<Team> deleteTeamEvent { get; set; }
    [Parameter] public EventCallback closeEvent { get; set; }

    [Inject] private IJSRuntime js { get; set; }

    private List<Member> members;
    [Inject] private IMemberService memberService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        members = targetTeam.Members;
        
        if (members == null)
        {
            Thread thread = new Thread(async o =>
            {
                members = await memberService.GetMembers();
                membersLoaded = true;
            } );
            thread.Start();
        }
        
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await js.InvokeVoidAsync("ExpandDetailedTeamCard");
        await Task.Delay(100);
        expanded = true;
        StateHasChanged();

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


    private void  loadMembers()
    {
        membersLoaded = true;
    }

    private async Task close()
    { 
        await js.InvokeVoidAsync("HideDetailedTeamCard",targetTeam.TeamId);
        expanded = false;
        await Task.Delay(350);
        await closeEvent.InvokeAsync();
    }
}