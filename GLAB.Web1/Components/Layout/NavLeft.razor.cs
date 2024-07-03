
using Glab.Domains.Models.Grades;
using Glab.Implementation.Services.Grades;
using Glab.Implementation.Services.Roles;
using Glab.Infrastructures.Storages.FacultiesStorages;
using GLAB.App.Teams;
using GLAB.App.Users;
using GLAB.Domains.Models.Teams;
using GLAB.Domains.Models.Users;
using GLAB.Implementation.Services.Laboratories;
using GLAB.Implementation.Services.Teams;
using GLAB.Implementation.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GLAB.Web1.Components.Layout
{
   
    public partial class NavLeft
    {
        
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private IUserService UserService { get; set; }
        private string Email = null;
        private string profile;
       

        private async Task loadData()
        {

            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = auth.User;

            var Profile = user.Claims.First(claim=>claim.Type.Equals("Profile"));

            profile = Profile.Value;

           


        }

        protected override async Task OnInitializedAsync()
        {
            await loadData();

        }

        

      

    }
}
