using GLAB.App.Memebers;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Models.Members;
using GLAB.Domains.Shared;
using GLAB.Infra.Storages.MembersStorages;
using System.Transactions;

namespace GLAB.Implementation.Services.Members
{
    public class MemberService : IMemberService
    {
        private readonly IMemberStorage memberStorage;
        private IMemberService _memberServiceImplementation;

        public MemberService(IMemberStorage memberStorage)
        {
            this.memberStorage = memberStorage;
        }

        public async ValueTask<Result> CreateMember(Member member)
        {

                try
                {
                List<ErrorCode> errorList = ValidateMemberForInsert(member);

                    if (errorList.Any())
                        return Result.Failure(errorList); 

                    await memberStorage.InsertMember(member);

                    return Result.Succes;
                }
                catch (Exception ex)
                {
                return Result.Failure(new string[] { "An error occurred while inserting the member." });
                }

            }

        public List<ErrorCode> ValidateMemberForInsert(Member member)
        {
            List<ErrorCode> errors = new List<ErrorCode>();

            if (string.IsNullOrWhiteSpace(member.MemberId))
                errors.Add(MemberError.IdEmpty);

            if (string.IsNullOrWhiteSpace(member.FirstName))
                errors.Add(MemberError.FirstNameEmpty);

            if (string.IsNullOrWhiteSpace(member.LastName))
                errors.Add(MemberError.LastNameEmpty);

            if (string.IsNullOrWhiteSpace(member.Email))
                errors.Add(MemberError.EmailEmpty);

           /* if (string.IsNullOrWhiteSpace(member.NIC))
                errors.Add(MemberError.NICEmpty);

            if (string.IsNullOrWhiteSpace(member.PhoneNumber))
                errors.Add(MemberError.PhoneNumberEmpty);

            if (member.Image is null)
                errors.Add(MemberError.PhotoEmpty);*/

                return errors;
        }

        public async Task<bool> updateMember(Member member)
        {
           return await memberStorage.UpdateMember( member);
        }

        public async Task<List<Member>> GetMembersByLab(string labId)
        {
            try
            {
                return await memberStorage.SelectMembersByLab(labId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async ValueTask<Result> SetMemberStatus(string id)
            {
                try
                {
                    
                    List<ErrorCode> errors = new List<ErrorCode>();
                        if (string.IsNullOrWhiteSpace(id))
                        {
                            errors.Add(MemberError.IdEmpty);
                            return Result.Failure(errors);
                        }

            await memberStorage.UpdateMemberStatus(id);

                        return Result.Succes;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during member removal: {ex }");
                    return Result.Failure(new[] { "An error occurred during member removal." });
                }
            }

            public async ValueTask<Result> SetMember(Member member)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        List<ErrorCode> errorList = await ValidateMemberForUpdate(member);
                        if (errorList.Any())
                            return Result.Failure(errorList.Select(e => e ).ToList());

                        await memberStorage.UpdateMember(member);
                        scope.Complete();
                        return Result.Succes;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting member: {ex }");
                        return Result.Failure(new[] { "An error occurred while setting the member." });
                    }
                
                }
            }
        public async ValueTask<List<ErrorCode>> ValidateMemberForUpdate(Member member)
            {
                List<ErrorCode> errors = new List<ErrorCode>();


                if (string.IsNullOrWhiteSpace(member.NIC))
                    errors.Add(MemberError.NICEmpty);

                if (string.IsNullOrWhiteSpace(member.PhoneNumber))
                    errors.Add(MemberError.PhoneNumberEmpty);

                 if (member.Image is null)
                    errors.Add(MemberError.PhotoEmpty);
             
                return errors;
            }


            public async ValueTask<List<Member>> GetMembers()
            {
                try
                {
                    List<Member> members = await memberStorage.SelectMembers();

                    if (members == null)
                    {
                        throw new Exception("Members list is null.");
                    }

                    return members;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting members: {ex }");
                    throw;
                }
            }


        //DALI ADD GETDERICTORS


        public async ValueTask<List<Member>> GetDirectors()
        {
            try
            {
                List<Member> members = await memberStorage.SelectDirector();

                if (members == null)
                {
                    throw new Exception("Members list is null.");
                }

                return members;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting members: {ex}");
                throw;
            }
        }


        //======================================================




        /*        public async ValueTask<Member> GetMemberById(string id)
                {

                        return await memberStorage.SelectMemberById(id);


                }*/

        public async ValueTask<Member> GetMemberByName(string name)
            {
                try
                {
                    return await memberStorage.SelectMemberByName(name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting member by name: {ex }");
                    throw;
                }
            }

            public async ValueTask<Member> GetMemberByEmail(string email)
            {
                try
                {
                    return await memberStorage.SelectMemberByEmail(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error selecting member by email: {ex }");
                    throw;
                }
            }



        public async Task<List<Member>> GetDirectorsByLaboratory(string labId)
        {
            try
            {
                return await memberStorage.SelectDirectorsByLaboratory(labId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }


    }
}
