using Glab.Domains.Models.Faculties;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Models.Teams;

namespace GLAB.Domains.Models.Laboratories;

public class Laboratory
{
    public string LaboratoryId { get; set; }
    public string DirectorId { get; set; }
    public string Name { get; set; }
    public string Adresse { get; set; }
    public string University { get; set; }
    public string FacultyId { get; set; }
    public string Acronyme { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string NumAgrement { get; set; }
    public DateTime CreationDate { get; set; }
    public DateOnly AgrementDate { get; set; }
    public byte[] Logo { get; set; } = new byte[] { };
    public string WebSite { get; set; }
    public List<Member> Members { get; set; }
    public List<Team> Teams { get; set; }
    public Faculty Faculte { get; set; }
 
    public LaboratoryStatus Status { get; set; }
  
}
