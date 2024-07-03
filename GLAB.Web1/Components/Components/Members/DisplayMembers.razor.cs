using GLAB.App.Memebers;
using GLAB.Domains.Models.Members;
using Microsoft.AspNetCore.Components;

namespace GLAB.Web1.Components.Components.Members;

partial class DisplayMembers
{

     [Inject] private IMemberService memberService { get; set; }
     [Parameter] public List<Member> members { get; set; }
     protected override async Task OnInitializedAsync()
     {
         if (members == null)
         {
             members=await memberService.GetMembers();
         }
     }
}