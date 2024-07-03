using Glab.Domains.Models.Faculties;
using Glab.Infrastructures.Storages.FacultiesStorages;
using GLAB.App.Laboratories;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Shared;
using Microsoft.AspNetCore.Components;
using System.IO;
using System.Net;

namespace GLAB.Web1.Components.Components
{
    
    public partial class CreateLaboByManagerComponent
    {
       
       [Inject]
        private IFacultyStorage facultyStorage { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        Result result;

        [Inject]
        private ILabService labService { get; set; }
       

        private CreateLaboByManagerModel newlabo = new CreateLaboByManagerModel();
        private List<Faculty> facultys = new List<Faculty>();

         private String failed = String.Empty;
         private String success = String.Empty;
        private bool hasErroe = false;

        protected override async Task OnInitializedAsync()
        {
            facultys = await facultyStorage.SelectFaculties();
        }

        private async Task save()
        {

            success = String.Empty;



            Laboratory labToCreate = new Laboratory()
            {
                LaboratoryId = Guid.NewGuid().ToString(),
                Name = newlabo.Name,
                Acronyme = newlabo.Acronyme,  
                CreationDate = DateTime.Now,
                Email = newlabo.Email,
                FacultyId = newlabo.Faculty,
                NumAgrement = newlabo.NumAgrement, 
                AgrementDate=newlabo.DateAgrement,
                Status = GLAB.Domains.Models.Laboratories.LaboratoryStatus.Bloqued,
                PhoneNumber= "Default",
                WebSite = "Default",
                University = "Default",
                Adresse = "Default",
                DirectorId = "Default"


            };

            result = await labService.CreateLaboratory(labToCreate);
            success = "The Laboratory is Created";

            navigationManager.NavigateTo("/");





        }

    }

}

