using GLAB.Domains.Models.Teams;
using GLAB.Domains.Shared;
namespace GLAB.App.Teams
{
    public interface ITeamService
    {

        ValueTask<List<Team>> GetTeams();
        ValueTask<Team> GetTeamById(string id);
        ValueTask<List<Team>> GetTeamsByLaboratory(string id);
        ValueTask<Team> GetTeamByAcronyme(string Acronyme);
        ValueTask<Team> GetTeamByName(string name);


        ValueTask<Result> SetTeamStatus(string TeamId, TeamStatus status);

        ValueTask<Result> SetTeam(Team team);

        /* I removed return type Result */
        ValueTask CreateTeam(Team team);


        Task<bool> RemoveTeam(Team team);
    }

}