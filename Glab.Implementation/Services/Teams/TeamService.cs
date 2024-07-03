
using System.Transactions;
using GLAB.Domains.Models.Teams;
using GLAB.Domains.Shared;
using System.ComponentModel.DataAnnotations;
using GLAB.App.Teams;
using GLAB.Implementation.Services.Laboratories;
using GLAB.Infra.Storages.TeamsStorages;
namespace GLAB.Implementation.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly ITeamStorage teamStorage;

        public TeamService(ITeamStorage teamStorage)
        {
            this.teamStorage = teamStorage;
        }
        /* the old version of method CreateTeam  */
        /*        public async ValueTask<Result> CreateTeam(Team team)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))

                    {
                        try

                        {

                            List<ErrorCode> errorList = ValidateTeamForInsert(team);

                            if (errorList.Any())

                                return Result.Failure(errorList.Select(e => e.Message).ToList());

                            await teamStorage.InsertTeam(team);

                            scope.Complete();

                            return Result.Succes;

                        }

                        catch (Exception ex)

                        {

                            Console.WriteLine($"Error setting laboratory: {ex.Message}");

                            return Result.Failure(new List<string> { "An error occurred while setting the laboratory." });

                        }

                    }

                }
        */

        /* the new version of method CreateTeam  */
        public async ValueTask CreateTeam(Team team)
        {
                    List<ErrorCode> errorList = validateTeamForInsert(team);
                    if (errorList.Any())
                    {
                        // Throw an exception with error messages
                        string errorMessage = string.Join(Environment.NewLine, errorList.Select(e => e.Message));
                        throw new ValidationException(errorMessage);
                    }
                    await teamStorage.InsertTeam(team);   
            
        }

        public async Task<bool> RemoveTeam(Team team)
        {
            try
            {
               return await teamStorage.deleteTeam(team);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async ValueTask<Team> GetTeamByAcronyme(string Acronyme)
        {
            /*Waiting Prof Sriti To show Us How To Handle This */
            return await teamStorage.SelectTeamByName(Acronyme);
        }

        public async ValueTask<Team> GetTeamById(string id)
        {
            /*Waiting Prof Sriti To show Us How To Handle This */
            return await teamStorage.SelectTeamById(id);
        }

        public async ValueTask<List<Team>> GetTeams()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                try
                {
                    List<Team> teams = await teamStorage.SelectTeams();


                    if (teams == null)
                    {
                        throw new Exception("Teams list is null.");
                    }
                    scope.Complete();
                    return teams;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting teams: {ex.Message}");
                    throw;
                }
        }

        public async ValueTask<List<Team>> GetTeamsByLaboratory(string id)
        {
         
            return await teamStorage.SelectTeamsByLaboratoryId(id);
        }

        public async ValueTask<Result> SetTeamStatus(string TeamId, TeamStatus status)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Check for valid ID before removing
                    if (string.IsNullOrWhiteSpace(TeamId))
                    {
                        throw new Exception("Invalid ID for removal");
                    }

                    await teamStorage.UpdateTeamStatus(TeamId, status);

                    scope.Complete();
                    return Result.Succes;
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(new [] { ex.Message });
            }
        }

        public async ValueTask<Result> SetTeam(Team team)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    List<ErrorCode> errorList = validateTeamForUpdate(team);
                    if (errorList.Any())
                        return Result.Failure(errorList);

                    await teamStorage.UpdateTeam(team);
                    scope.Complete();
                    return Result.Succes;
                }
                catch (Exception ex)
                {
                    // Handle the exception if needed
                    Console.WriteLine($"Error setting team: {ex.Message}");
                    return Result.Failure(new[] { "An error occurred while setting the Team." });
                }
            }
        }


        /* Missing Class TeamErrorCodes */

        private List<ErrorCode> validateTeamForInsert(Team team)

        {
            
            List<ErrorCode> errors = new List<ErrorCode>();


            if (!Enum.IsDefined(typeof(TeamStatus), team.Status) || string.IsNullOrWhiteSpace(team.Status.ToString()))
                errors.Add(TeamErrorCodes.StatusEmpty);

            if (string.IsNullOrWhiteSpace(team.LaboratoryId))

                errors.Add(LaboratoryErrorsService.LaboratoryIdEmpty);

            /*           
             *          ??????????????????????? what about await make method async or move these conditions above          
             *           if ( await  teamStorage.ExistId(team.TeamId))

                            errors.Add(TeamErrors.IdExist);


                        if (await LaboratoryExists(Laboratory.LaboratoryId))

                            errors.Add(LaboratoryErrorsService.IdExist);*/

            return errors;

        }

        private List<ErrorCode> validateTeamForUpdate(Team team)
        {
            List<ErrorCode> errors = new List<ErrorCode>();


            if (!Enum.IsDefined(typeof(TeamStatus), team.Status) || string.IsNullOrWhiteSpace(team.Status.ToString()))
                errors.Add(TeamErrorCodes.StatusEmpty);


            if (string.IsNullOrWhiteSpace(team.TeamId))

                errors.Add(TeamErrorCodes.IdEmpty);


            return errors;
        }

        public async ValueTask<Team> GetTeamByName(string name)
        {
            return await teamStorage.SelectTeamByName(name);
        }


    }
}