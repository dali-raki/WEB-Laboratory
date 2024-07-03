using System.ComponentModel.DataAnnotations;

namespace GLAB.Web1.Components.Components
{
    public partial class CreateLaboByManagerModel
    {

        [Required(ErrorMessage = "The Name is mandatory")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Acronyme is mandotory")]
        public string Acronyme { get; set; }
        [Required(ErrorMessage = "Email is mandotory")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Faculty is mandotory")]
        public string Faculty { get; set; }


        [Required(ErrorMessage = "Number Agrement is mandotory")]
        public string NumAgrement { get; set; }

        [Required(ErrorMessage = "Date agrement is mandotory")]

        public DateOnly DateAgrement { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public string Address { get; set; } 

        public string University { get; set; }

        public string PhoneNumber { get; set;} 

        public string Website { get; set; }

        public string LaboratoryId { get; set; }

    }
}
