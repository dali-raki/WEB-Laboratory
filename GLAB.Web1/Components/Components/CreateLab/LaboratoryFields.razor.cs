
using Glab.App.Grades;
using Glab.Domains.Models.Grades;
using Glab.Ui.Members;
using Glab.Ui.Members.Models;
using GLAB.App.Laboratories;
using GLAB.App.Memebers;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IO;
using System.Security.Claims;

namespace GLAB.Web1.Components.Components.CreateLab
{
    public partial class LaboratoryFields
    {
        [Inject] private IMemberService memberService { get; set; }
        [Inject] private ILabService labService { get; set; }
        private CreateLabAdminModel labobyadmin = new CreateLabAdminModel();
        private List<Member> members;
        private Laboratory labInfo;
        private string adminEmail = "email";
        private String failed = String.Empty;
        private String success = null;
        private bool hasErroe = false;
        private Result result;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private void photoChanged(byte[] image) => labobyadmin.Logo = image;

        private async Task createLabByAdmin()
        {


            try
            {
                Laboratory labToUpdate = new Laboratory()
                {
                    DirectorId = labobyadmin.DirectorId,
                    Adresse = labobyadmin.Adresse,
                    Logo = labobyadmin.Logo,
                    Email = adminEmail,
                    PhoneNumber = labobyadmin.PhoneNumber,
                    University = "Biskra",
                    WebSite = labobyadmin.WebSite
                };

                

                await labService.SetLaboratory(labToUpdate);
                success = "The Information is Saved";
            }
            catch (Exception ex)
            {

                throw;
            }
         ;
        }

        private async Task loadData()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            adminEmail = auth.User.Claims.ToList().Find(claim => claim.Type == ClaimTypes.Email).Value;
            string labId = auth.User.Claims.ToList().Find(claim => claim.Type == "LabId").Value;
            members = await memberService.GetDirectorsByLaboratory(labId);
            labInfo = await labService.GetLaboratoryInformations(adminEmail);
            labobyadmin.Adresse = labInfo.Adresse;
            labobyadmin.PhoneNumber = labInfo.PhoneNumber;
            labobyadmin.WebSite = labInfo.WebSite;
            labobyadmin.DirectorId = labInfo.DirectorId;
        }

        protected override async Task OnInitializedAsync()
        {
            await loadData();
        }
    }
}
