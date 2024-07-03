using System.ComponentModel.DataAnnotations;

namespace Glab.Ui.Members.Models
{
    public class ConfirmRegistrationModel
    {
        [Required(ErrorMessage = "Vous devez donner le NIC.")]
        public string NIC { get; set; }

        [Required(ErrorMessage = "Vous devez donner le PhoneNumber.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vous devez ajouter un photo.")]
        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Vous devez donner le mot de pass.")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Vous devez donner le mot de pass.")]
        public String ConfirmPassword { get; set; }

    
    }
}