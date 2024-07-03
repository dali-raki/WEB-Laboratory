using GLAB.Domains.Shared;

namespace GLAB.Domains.Models.Laboratories;

public static class LaboratoryErrors
{
    public static ErrorCode IdEmpty { get; } =
        new ErrorCode("LaboratoryErrors.IdEmpty", "The id cannot be empty");

    public static ErrorCode StatusEmpty { get; } =
        new ErrorCode("LaboratoryErrors.StatusEmpty", "The laboratory status cannot be empty");

    public static ErrorCode AcronymeEmpty { get; } =
        new ErrorCode("LaboratoryErrors.AcronymeEmpty", "The acronym cannot be empty");

    public static ErrorCode NameEmpty { get; } =
        new ErrorCode("LaboratoryErrors.NameEmpty", "The name cannot be empty");

    public static ErrorCode AddressEmpty { get; } =
        new ErrorCode("LaboratoryErrors.AddressEmpty", "The address cannot be empty");

    public static ErrorCode UniversityEmpty { get; } =
        new ErrorCode("UniversityEmpty", "The university name cannot be empty");

    public static ErrorCode PhoneNumberEmpty { get; } =
        new ErrorCode("LaboratoryErrors.PhoneNumberEmpty", "The phone number cannot be empty");

    public static ErrorCode EmailEmpty { get; } =
        new ErrorCode("LaboratoryErrors.EmailEmpty", "The email address cannot be empty");

    public static ErrorCode AgreementNumberEmpty { get; } =
        new ErrorCode("LaboratoryErrors.AgreementNumberEmpty", "The agreement number cannot be empty");

    public static ErrorCode CreationDateInvalid { get; } =
        new ErrorCode("LaboratoryErrors.InvalidCreationDate", "Invalid creation date");

    public static ErrorCode LogoEmpty { get; } =
        new ErrorCode("LaboratoryErrors.LogoEmpty", "The logo cannot be empty");

    public static ErrorCode WebSiteEmpty { get; } =
        new ErrorCode("WebSiteEmpty", "The website address cannot be empty");

    public static ErrorCode LaboratoryIdExists { get; } =
        new ErrorCode("LaboratoryErrors.LaboratoryIdExists", "This laboratory ID already exists");
    public static ErrorCode LaboratoryAcronymExists { get; } =
        new ErrorCode("LaboratoryErrors.LaboratoryAcronymExists", "This laboratory Acronyme already exists");
    public static ErrorCode LaboratoryAgrementExists { get; } =
        new ErrorCode("LaboratoryErrors.LaboratoryAgrementExists", "A laboratory with this agrement number already exists");
    public static ErrorCode LaboratoryEmailExists { get; } =
        new ErrorCode("LaboratoryErrors.LaboratoryEmailExists", "A laboratory with this Email  already exists");



}