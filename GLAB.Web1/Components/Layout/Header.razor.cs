using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Claims;


namespace GLAB.Web1.Components.Layout
{

    public partial class Header

    {
         [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
       
        private string adminEmail = null;
        private async Task loadData()
        {

            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            adminEmail = auth.User.Claims.ToList().Find(claim => claim.Type == ClaimTypes.Email).Value;
        }

        protected override async Task OnInitializedAsync()
        {
            await loadData();
        }


    }
}
