using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ids.Shared.AttributeValidations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class RequiredIfAttribute : ValidationAttribute, IClientModelValidator
{
    private string PropertyName { get; set; }
    private object Value { get; set; }

    public RequiredIfAttribute(string propertyName, object value, string errorMessage = "")
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        Value = value;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();
        var proprtyvalue = type.GetProperty(PropertyName)?.GetValue(instance, null);
        if (proprtyvalue != null && proprtyvalue.ToString() == Value.ToString() && value == null)
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-error", error);
    }
}