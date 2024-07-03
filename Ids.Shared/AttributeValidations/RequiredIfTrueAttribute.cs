using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ids.Shared.AttributeValidations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class RequiredIfTrueAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly bool condition;

    public RequiredIfTrueAttribute(bool Condition)
    {
        condition = Condition;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return value is null || condition == false
            ? new ValidationResult(ErrorMessage)
            : ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-error", error);
    }
}