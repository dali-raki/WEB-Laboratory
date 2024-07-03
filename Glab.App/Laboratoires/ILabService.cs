using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Shared;


namespace GLAB.App.Laboratories
{
    public interface ILabService
    {
        Task<List<Laboratory>> GetLaboratories();

        Task<Laboratory> GetLaboratoryById(string id);
        Task<Laboratory> GetLaboratoryByName(string name);

        Task<Result> SetLaboratoryStatus(string id);

        Task<Result> SetLaboratory(Laboratory laboratory);

        Task<Result> CreateLaboratory(Laboratory laboratory);

        Task<Laboratory?> GetLaboratoryInformations(string email);

        Task<Laboratory> GetLaboratoryByUser(string userId);
    }

}