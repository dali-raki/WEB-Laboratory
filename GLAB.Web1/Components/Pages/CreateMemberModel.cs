using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Components.Pages
{
    public class CreateMemberModel
    {
        [Required(ErrorMessage = "First name is mandatory")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is mandatory")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is mandatory")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is mandatory")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Team is mandatory")]
        public string Team { get; set; }
    }
}
