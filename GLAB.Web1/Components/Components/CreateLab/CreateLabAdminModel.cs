using System.ComponentModel.DataAnnotations;

namespace GLAB.Web1.Components.Components.CreateLab
{
    public class CreateLabAdminModel
    {
        [Required(ErrorMessage = "The Adresse is mandatory")]
        public string Adresse { get; set; }
        [Required(ErrorMessage = "The Director is  mandatory")]
        public string DirectorId { get; set; }

        [Required(ErrorMessage = " The PhoneNumber mandatory")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The Logo mandatory")]
        public byte[] Logo { get; set; } = new byte[] { };

        [Required(ErrorMessage = " The Website mandatory")]
        public string WebSite { get; set; }
    }
}
