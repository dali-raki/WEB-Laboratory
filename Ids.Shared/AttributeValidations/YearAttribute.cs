using System.ComponentModel.DataAnnotations;

namespace Ids.Shared.AttributeValidations;

public class YearAttribute : RegularExpressionAttribute
{
    private const string ProjectYearRule = "[0-9]{4}";

    public YearAttribute() : base(ProjectYearRule)
    {
    }
}