using Glab.Domains.Models.Roles;
using System.ComponentModel.DataAnnotations;

namespace Glab.Ui.Members.Models
{
    public class CreateMemberModels
    {
        [Required(ErrorMessage = "The first name is required.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "The last name is required.")]
        public string Prenom { get; set; }

        [EmailAddress(ErrorMessage = "Email adress not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You have to choose a team.")]
        public string Equipe { get; set; }

        [Required(ErrorMessage = "You have to choose a grade.")]
        public string GradeId { get; set; }

        // public List<Role> SelectedRoles { get; set; }



    }

}
