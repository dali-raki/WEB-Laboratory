using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ids.Shared.AttributeValidations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class YearGreatThanAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly string comparisonProperty;

    public YearGreatThanAttribute(string comparisonProperty) =>
        this.comparisonProperty = comparisonProperty;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        ErrorMessage = ErrorMessageString;
        int currentValue;
        try
        {
            currentValue = Convert.ToInt16(value);
        }
        catch
        {
            return new ValidationResult("Entrez une année valide");
        }

        var property = validationContext.ObjectType.GetProperty(comparisonProperty);

        ArgumentNullException.ThrowIfNull(property, "Propriété {property} non trouvée");

        int comparisonValue;
        try
        {
            comparisonValue = Convert.ToInt16(property.GetValue(validationContext.ObjectInstance));
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