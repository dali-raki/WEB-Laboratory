
using GLAB.Domains.Shared;

namespace GLAB.Implementation.Services.Laboratories
{
    public class LaboratoryErrorsService
    {
        public static ErrorCode LaboratoryIdEmpty { get; } =
           new ErrorCode("LaboratoryErrors.LaboratoryIdEmpty", "The laboratory's id is Empty");


    }
}