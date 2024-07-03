using GLAB.App.Memebers;
using GLAB.Domains.Models.Members;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace GLAB.Web1.Components.Components.Members;

partial class DisplayMembersCards
{
    private string searchedMemberName;
    private ElementReference searchBar;
    [Inject] private IMemberService memberService { get; set; }
    [Inject] private AuthenticationStateProvider authState { get; set; }
    public List<Member> members =new (),searchedMembers=new ();
    private Timer searchTimer;
    

    protected override async Task OnInitializedAsync()
    {
       var auth =await authState.GetAuthenticationStateAsync();
      var labIdClaim= auth.User.Claims.First(claim => claim.Type.Equals("LabId"));
      
      if (labIdClaim != null)
      {
          members = await memberService.GetMembersByLab(labIdClaim.Value);
          searchedMembers = members;
      }
    }

    private async Task showAddMemberPopup()
    {

    }

    private  void  TimerCallback(Object o)
    {
        search();

    }

    private async Task search()
    {
        
        searchedMembers = members.FindAll(member => string.Concat(member.FirstName +" " +member.LastName).ToUpper().Contains(searchedMemberName.ToUpper()));
        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    

    private async Task searchMember()
    {
        
        if (String.IsNullOrEmpty(searchedMemberName))
        {
            searchedMembers = members;
            return;
        }
        
        if (searchTimer != null)
        {
            await searchTimer.DisposeAsync();
          
        }
        
        searchTimer = new Timer(TimerCallback, null, 0, 300);

    }
}