using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ids.Shared.AttributeValidations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class UnivEmailAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly string domainName;

    public UnivEmailAttribute(string domainName)
    {
        this.domainName = domainName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string entry = value is null ? string.Empty : value.ToString().ToLower();
        string regex = $"^[A-Za-z0-9._%+-]+@{domainName}$";

        return !Regex.IsMatch(entry, regex)
            ? new ValidationResult($"Le compte email doit appartenir au domain @{domainName}")
            : ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-error", error);
    }
}