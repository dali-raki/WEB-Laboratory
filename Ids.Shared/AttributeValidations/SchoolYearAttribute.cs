using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ids.Shared.AttributeValidations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SchoolYearAttribute : ValidationAttribute, IClientModelValidator
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("Spécifiez une année universitaire valide. ex 2020/2021");

        string entry = value?.ToString();

        if (!Regex.IsMatch(entry, "[0-9]{4}/[0-9]{4}") || entry.Length != 9)
            return new ValidationResult("Année universitaire non valide. ex 2020/2021");

        string startYear = entry.Substring(0, 4);
        string endYear = entry.Substring(5, 4);

        return Convert.ToInt16(endYear) != Convert.ToInt16(startYear) + 1
            ? new ValidationResult("Les années doivent être consécutives. ex 2020/2021")
            : ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-error", error);
    }
}