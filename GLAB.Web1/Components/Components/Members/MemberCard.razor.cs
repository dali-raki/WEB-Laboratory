using GLAB.Domains.Models.Members;
using Microsoft.AspNetCore.Components;

namespace GLAB.Web1.Components.Components.Members;

partial class MemberCard
{
    [Parameter] public Member member { get; set; }
    private string grade;
    protected override async Task OnInitializedAsync()
    {
        var Grade  = (Grades)int.Parse(member.GradeId);
        grade = Grade.ToString();
    }
}