using GLAB.Domains.Models.Members;

namespace GLAB.Domains.Models.Teams;

public class Team
{
    public string LaboratoryId { get; set; }
    public string TeamId { get; set; }
    public string TeamName { get; set; }
    public string TeamAcronyme { get; set; }
    public TeamStatus Status { get; set; }
    public List<Member> Members { get; set; } // Ajout de la liste des membres
    public string TeamLeaderId { get; set; }


    public Team()
    {
        Members = new List<Member>();
    }
}