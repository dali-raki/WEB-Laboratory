using Glab.Domains.Models.Roles;

namespace GLAB.Domains.Models.Members;

public class Member
{
    public string MemberId { get; set; }
    public string TeamId { get; set; }
    public string GradeId { get; set; }
    public string LaboratoryId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string NIC { get; set; }
    public string PhoneNumber { get; set; }
    public byte[] Image { get; set; } = new byte[] { };
    public MemberStatus Status { get; set; }
    public List<Role>Roles { get; set; }
    public string Grade;
    
    public Member()
    {
        Roles = new List<Role>();
    }
        
  

  

}

public enum Grades
{
    PROF=1,
    MCA=2,
    MCB=3,
    MAA=4,
    MAB=5,
    PHDSTUDENT=6
}