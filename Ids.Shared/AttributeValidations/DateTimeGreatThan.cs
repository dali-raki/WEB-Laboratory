using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ids.Shared.AttributeValidations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class DateTimeGreatThan(string comparisonProperty) : ValidationAttribute, IClientModelValidator
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        ErrorMessage = ErrorMessageString;
        DateTime? currentValue;
        try
        {
            currentValue = Convert.ToDateTime(value);
        }
        catch
        {
            return new ValidationResult("Entrez une année valide");
        }

        var property = validationContext.ObjectType.GetProperty(comparisonProperty);

        ArgumentNullException.ThrowIfNull(property, "Propriété {property} non trouvée");

        DateTime? comparisonValue;
        try
        {
            comparisonValue = Convert.ToDateTime(property.GetValue(validationContext.ObjectInstance));
        }
        catch
        {
            return new ValidationResult("Vérifiez que les données saisies sont valides");
        }

        return currentValue < comparisonValue ?
            new ValidationResult(ErrorMessage) :
            ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-error", error);
    }
}