using GLAB.Domains.Models.Members;
using GLAB.Domains.Shared;


namespace GLAB.App.Memebers
{
    public interface IMemberService
    {
        ValueTask<List<Member>> GetMembers();
/*        ValueTask<Member> GetMemberById(string id);*/
        ValueTask<Member> GetMemberByEmail(string email);
        ValueTask<Member> GetMemberByName(string name);
        ValueTask<Result> CreateMember(Member member);
        ValueTask<Result> SetMemberStatus(string id);
        ValueTask<Result> SetMember(Member member);
        ValueTask<List<ErrorCode>> ValidateMemberForUpdate(Member member);
        List<ErrorCode> ValidateMemberForInsert(Member member);

        Task<bool> updateMember(Member member);

        Task<List<Member>> GetMembersByLab(string labId);

        Task<List<Member>> GetDirectorsByLaboratory(string labId);
    }
}