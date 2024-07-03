using Glab.Domains.Models.Roles;
using GLAB.Domains.Models.Members;

namespace GLAB.Infra.Storages.MembersStorages;

public interface IMemberStorage
{
    Task<List<Member>> SelectMembers();

    Task<List<Member>> SelectDirector();

    Task InsertMember(Member member);

    Task<bool> UpdateMember(Member member);

    Task UpdateMemberStatus(string id);

    Task<bool> MemberExistsById(string id);
    Task<Member> SelectMemberByEmail(string email);
    Task<Member> SelectMemberByName(string name);
    Task AddMemberRole(string memberId, string roleId);
    Task RemoveMemberRole(string memberId, string roleId);
    Task<List<Role>> SelectRolesOfMember(String  memberId);

    Task<List<Member>> SelectDirectorsByLaboratory(string laboId);


    Task<List<Member>> SelectMembersByLab(string labId);
}
