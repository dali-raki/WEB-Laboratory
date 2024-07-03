using System.ComponentModel.DataAnnotations;

namespace GLAB.Web1.Components.Members.Models
{
    public class RegisterMemberModel
    {
        [Required(ErrorMessage = "the PhoneNumber is mandatory")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "the NIC is mandatory")]
        public string NIC { get; set; }
        [Required(ErrorMessage = "the GradeID is mandatory")]
        public string GradeId { get; set; }
        [Required(ErrorMessage = "Password is mandatory")]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "Confirm password  is mandatory")]
        public string ConfirmPassWord { get; set; }

    }
}
